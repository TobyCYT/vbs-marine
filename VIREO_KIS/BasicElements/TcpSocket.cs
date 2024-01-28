using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;

namespace VIREO_KIS.BasicElements
{
    public class TcpSocket
    {
        private Socket _socket;
        private int _port;
        private string _address;
        public TcpSocket(string target_ip, int port)
        {
            _port = port;
            _address = target_ip;
        }

        public bool connect()
        {
            bool isSuccessful = false;
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_address, _port);
                isSuccessful = true;
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return isSuccessful;
        }

        public void sendQuery(MODEL_TYPE modelType, SEARCH_TYPE searchType, in string query, ref double[] embeddingResult)
        {
            byte[] send_data = Encoding.UTF8.GetBytes(query);
            byte[] result_size_buffer = new byte[4];
            //_socket.Send(model)
            //send socket of 1 byte
            int systemType = 1; // trec = 0, marine = 1
            //string a = ((int)systemType).ToString();
            //string b = ((int)modelType).ToString();
            //string c = ((int)searchType).ToString();
            _socket.Send(Encoding.UTF8.GetBytes(((int)systemType).ToString())); //1 byte


            _socket.Send(Encoding.UTF8.GetBytes(((int)modelType).ToString())); //1 byte
            _socket.Send(Encoding.UTF8.GetBytes(((int)searchType).ToString())); //1 byte
            _socket.Send(send_data); // send all
            _socket.Receive(result_size_buffer, 4, SocketFlags.Partial);
            int result_size = BitConverter.ToInt32(result_size_buffer, 0);
            for (int i = 0; i < result_size; i++)
            {
                byte[] index_buffer = new byte[4];
                byte[] emb_values_length_bytes = new byte[2];

                // receive index
                _socket.Receive(index_buffer, 4, SocketFlags.None); // 4 byte2
                int index = BitConverter.ToInt32(index_buffer, 0); 

                // receive length of score
                _socket.Receive(emb_values_length_bytes, 2, SocketFlags.None); // 2 bytes
                int emb_value_buffer_length = BitConverter.ToInt16(emb_values_length_bytes, 0);
                // receive data
                byte[] temp_score = new byte[emb_value_buffer_length]; // emb_value_buffer_length bytes
                _socket.Receive(temp_score, emb_value_buffer_length, SocketFlags.None);

                embeddingResult[index] += Convert.ToDouble(Encoding.UTF8.GetString(temp_score));

            }

        }

        public void disconnect()
        {
            if (_socket.IsBound)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
        }
    }
}
