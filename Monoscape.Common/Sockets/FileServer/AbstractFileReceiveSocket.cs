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
using System.Threading;
using Monoscape.Common.Model;
using System.Runtime.Serialization.Formatters.Binary;
using Monoscape.Common.Exceptions;

namespace Monoscape.Common.Sockets.FileServer
{
    /// <summary>
    /// Implement this class in the context of the end point.
    /// </summary>
    public abstract class AbstractFileReceiveSocket
    {
        private Socket serverSocket;
        private IPAddress ipAddress;
        private int port;
        private string fileStorePath;
        private bool socketOpen = false;
        private Thread thread;
        private Socket clientSocket;

        protected abstract string SocketName { get; }

        public AbstractFileReceiveSocket(string fileStorePath, IPAddress ipAddress, int port)
        {
            if (fileStorePath.Last() != Path.DirectorySeparatorChar)
                this.fileStorePath = fileStorePath + Path.DirectorySeparatorChar;
            else
                this.fileStorePath = fileStorePath;
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Open()
        {
            Log.Debug(this, "Starting " + SocketName + " thread...");
            thread = new Thread(new ThreadStart(OpenSocket));
            thread.Start();
            Log.Debug(this, SocketName + " started");
        }

        private void OpenSocket()
        {
            socketOpen = true;           
            ReceiveFile();            
        }

        public void Close()
        {
            socketOpen = false;
            CloseSockets();
            thread.Abort();
        }

        private void ReceiveFile()
        {
            try
            {
                IPEndPoint ipEnd = new IPEndPoint(ipAddress, port);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                serverSocket.Bind(ipEnd);                
                serverSocket.Listen(100);

                while (socketOpen)
                {
                    // Wait for the next connection
                    clientSocket = serverSocket.Accept();

                    // Read File Header
                    // FileHeader: [fileNameInBytesLength (4bytes) | fileNameInBytes | fileSizeInBytesLength (8bytes) | fileSizeInBytes]
                    byte[] fileNameInBytesLengthInBytes = new byte[4];
                    Log.Debug(this, "Reading file name length in bytes");
                    int bytesRead = clientSocket.Receive(fileNameInBytesLengthInBytes, 4, SocketFlags.None);
                    if (bytesRead == 4)
                    {
                        int fileNameInBytesLength = BitConverter.ToInt32(fileNameInBytesLengthInBytes, 0);
                        Log.Debug(this, "File name length in bytes: " + fileNameInBytesLength);

                        Log.Debug(this, "Reading file name");
                        byte[] fileNameInBytes = new byte[fileNameInBytesLength];
                        bytesRead = clientSocket.Receive(fileNameInBytes, fileNameInBytesLength, 0);
                        if (bytesRead == fileNameInBytesLength)
                        {
                            string fileName = Encoding.ASCII.GetString(fileNameInBytes);
                            Log.Debug(this, "File name: " + fileName);

                            Log.Debug(this, "Reading file size length in bytes");
                            byte[] fileSizeInBytesLengthInBytes = new byte[8];
                            bytesRead = clientSocket.Receive(fileSizeInBytesLengthInBytes, 8, SocketFlags.None);
                            if (bytesRead == 8)
                            {
                                int fileSizeInBytesLength = BitConverter.ToInt32(fileSizeInBytesLengthInBytes, 0);
                                Log.Debug(this, "File size length in bytes: " + fileSizeInBytesLength);

                                Log.Debug(this, "Reading file size length");
                                byte[] fileSizeInBytes = new byte[fileSizeInBytesLength];
                                bytesRead = clientSocket.Receive(fileSizeInBytes, 8, SocketFlags.None);
                                if (bytesRead == 8)
                                {
                                    long fileSize = BitConverter.ToInt32(fileSizeInBytes, 0);
                                    Log.Debug(this, "File size: " + fileSize);

                                    if (!Directory.Exists(fileStorePath))
                                        Directory.CreateDirectory(fileStorePath);
                                    string filePath = Path.Combine(fileStorePath, fileName);

                                    // Read File Content
                                    long buffer = 65536; // Block size = 64K
                                    byte[] fileData = new byte[fileSize];
                                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                                    {
                                        bytesRead = 0;
                                        long totalBytesRead = 0;
                                        long bytesToRead = buffer;

                                        Log.Debug(this, "Reading file content... ");
                                        while (bytesToRead > 0)
                                        {
                                            bytesRead = clientSocket.Receive(fileData, (int)totalBytesRead, (int)buffer, SocketFlags.None);
                                            if (bytesRead == 0)
                                                break;

                                            fileStream.Write(fileData, (int)totalBytesRead, (int)bytesRead);

                                            totalBytesRead = totalBytesRead + bytesRead;
                                            bytesToRead = fileSize - totalBytesRead;
                                            if (bytesToRead < buffer)
                                                buffer = bytesToRead;

                                            Log.Debug(this, "Received bytes: " + (fileSize - bytesToRead));
                                        }
                                    }
                                    Log.Debug(this, "File " + fileName + " written to application store");
                                }
                                else
                                {
                                    throw new MonoscapeException("File header is not valid, could not receive file.");
                                }
                            }
                            else
                            {
                                throw new MonoscapeException("File header is not valid, could not receive file.");
                            }
                        }
                        else
                        {
                            throw new MonoscapeException("File header is not valid, could not receive file.");
                        }
                    }
                    else
                    {
                        throw new MonoscapeException("File header is not valid, could not receive file.");
                    }
                }
            }
            catch (Exception e)
            {
				if(socketOpen)
				{
                	Log.Error(this, "File receiving failed", e);
					if(serverSocket.Connected)
                		throw e;
				}
            }
            finally
            {
                CloseSockets();
            }
        }

        private void CloseSockets()
        {
            socketOpen = false;
            if (serverSocket != null)
                serverSocket.Close();
            if (clientSocket != null)
                clientSocket.Close();
        }

        private static object DeSerialize_(byte[] dataBuffer)
        {
            using (MemoryStream stream = new MemoryStream(dataBuffer))
            {
                stream.Position = 0;
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        private Object DeSerialize(byte[] dataBuffer)
        {
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            mem.Write(dataBuffer, 0, dataBuffer.Length);
            mem.Seek(0, SeekOrigin.Begin);
            return bin.Deserialize(mem);
        }
    }
}
