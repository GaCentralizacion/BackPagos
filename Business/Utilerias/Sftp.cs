using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilerias
{
    public class Sftp
    { 
        public string host {get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string workingdirectory { get; set; }
        public string uploadfile { get; set; }
        public int port { get; set; }

        public bool subirArchivoRutaFisica()
        {
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Debug.WriteLine("I'm connected to the client");


                        using (var fileStream = new FileStream(uploadfile, FileMode.Open))
                        {

                            client.BufferSize = 4 * 1024; // bypass Payload error large files
                            client.UploadFile(fileStream, Path.GetFileName(uploadfile));
                        }

                    }
                    else
                    {

                        Debug.WriteLine("I couldn't connect");
                        return false;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool subirArchivoArchivoBinarizado(byte[] file,string fileName)
        {
            try
            {
                using (var client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Debug.WriteLine("I'm connected to the client");

                        using (var ms = new MemoryStream(file))
                        {
                            client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                            client.UploadFile(ms, fileName);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("I couldn't connect");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
