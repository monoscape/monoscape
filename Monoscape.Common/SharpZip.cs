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

// Reference: http://www.icsharpcode.net/OpenSource/SharpZipLib/

using System;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Monoscape.Common
{
	public static class SharpZip
	{
		public static bool FileExistsInRoot(string zipFilePath, string fileName, bool ignoreCase)
		{
			using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath))) 
			{		
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry()) != null) 
				{				
					//Log.Debug(typeof(SharpZip), theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
					string fileName_ = Path.GetFileName (theEntry.Name);		
		
					if((!ignoreCase) && ((directoryName.Length == 0) && fileName_.Equals(fileName))
                        || ((ignoreCase) && ((directoryName.Length == 0) && fileName_.ToLower().Equals(fileName.ToLower()))))
					{
						Log.Debug(typeof(SharpZip), "File found: " + fileName);
						return true;
					}
				}
			}
			return false;
		}
		
		public static void Extract(string extractPath, string filePath)
		{
			using (ZipInputStream s = new ZipInputStream(File.OpenRead(filePath))) 
			{		
				Log.Debug(typeof(SharpZip), "Extracting zip file: " + Path.GetFileName(filePath));
				
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry()) != null) 
				{				
					//Log.Debug(typeof(SharpZip), theEntry.Name);
				
					string directoryName = Path.GetDirectoryName(theEntry.Name);
				    string fileName      = Path.GetFileName(theEntry.Name);
					
					// create directory
					if (directoryName.Length > 0) 
					{
						Directory.CreateDirectory (Path.Combine(extractPath, directoryName));
					}
				
					if (fileName != String.Empty) 
					{
						using (FileStream streamWriter = File.Create(Path.Combine(extractPath, theEntry.Name))) 
						{					
							int size = 2048;
							byte[] data = new byte[2048];
							while (true) 
							{
								size = s.Read (data, 0, data.Length);
								if (size > 0) 
								{
									streamWriter.Write (data, 0, size);
								} 
								else 
								{
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}

