/*
 *  Copyright 2013 Monoscape
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 *  History: 
 *  2011/11/10 Imesh Gunaratne <imesh@monoscape.org> Created.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Monoscape.CloudController.External.Api.Sockets
{
    /// <summary>
    /// Implement this class in the context of the end point.
    /// </summary>
    public abstract class AbstractFileTransferSocket
    {
        private IPAddress ipAddress;
        private int port;

        public AbstractFileTransferSocket(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void SendFile(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            SendFile(fileStream, Path.GetFileName(filePath));
        }

        public void SendFile(Stream fileStream, string fileName)
        {                       
            byte[] fileNameInBytes = Encoding.ASCII.GetBytes(fileName);
            byte[] fileNameInBytesLength = BitConverter.GetBytes(fileNameInBytes.Length);

            if (fileNameInBytesLength.Length > 4)
                throw new Exception("File name length is too long. Please reduce the file name length and try again.");

            using (fileStream)
            {
                // FileHeader: [fileNameInBytesLength (4bytes) | fileNameInBytes | fileSizeInBytesLength (8bytes) | fileSizeInBytes]                
                long fileSize = fileStream.Length;
                byte[] fileSizeInBytes = BitConverter.GetBytes(fileSize);
                byte[] fileSizeInBytesLength = BitConverter.GetBytes(fileSizeInBytes.Length);
                int headerSize = 4 + fileNameInBytes.Length + 8 + fileSizeInBytes.Length;
                byte[] fileHeader = new byte[headerSize];

                fileNameInBytesLength.CopyTo(fileHeader, 0);
                fileNameInBytes.CopyTo(fileHeader, 4);
                fileSizeInBytesLength.CopyTo(fileHeader, (4 + fileNameInBytes.Length));
                fileSizeInBytes.CopyTo(fileHeader, (8 + 4 + fileNameInBytes.Length));
            
                // Starting FileTransferServerSocket...
                IPEndPoint ipEnd = new IPEndPoint(ipAddress, port);
                Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                clientSock.Connect(ipEnd);

                // Send File Header
                clientSock.Send(fileHeader, SocketFlags.None);
           
                long blockSize = 1024 * 64; // Block size = 64K
                byte[] fileData = new byte[fileSize];
                long bytesRead = 0;
                long totalBytesRead = 0;
                long bytesToRead = blockSize;
                // Reading file
                                
                while (bytesToRead > 0)
                {
                    bytesRead = fileStream.Read(fileData, (int)totalBytesRead, (int)blockSize);
                    if (bytesRead == 0)
                        break;

                    // Send File Blocks                    
                    clientSock.Send(fileData, (int)totalBytesRead, (int)bytesRead, SocketFlags.None);

                    totalBytesRead = totalBytesRead + bytesRead;
                    bytesToRead = fileSize - totalBytesRead;
                    if (bytesToRead < blockSize)
                        blockSize = bytesToRead;                   
                }                
            }                                  
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        private byte[] Serialize(Object obj)
        {
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            bin.Serialize(mem, obj);
            return mem.GetBuffer();
        }
    }
}
