using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Sensor_Database_Connection
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connection To Serial Port...
            SerialPort myPort = new SerialPort();
            myPort.BaudRate = 9600;
            myPort.PortName = "COM3";
            myPort.Open();

            //Sensor's PreConfigured Values...
            int idParking_Space = 1;
            int Floor = 1;
            int Parking_Slot = 1;
            string Status = "Occupied";
            //Sensor is Fixed at a prespecified position in real world, so each sensor should have its own idParking_Space
            //Floor Number and Parking_Slot

            while (true)
            {
                string data_rx = myPort.ReadLine();
                int data = Convert.ToInt32(data_rx);
                if (data <= 3)
                {
                    Console.WriteLine("Threshold Value == Car Parked!!");

                    //Now Connection To Database....
                    string ConnectionString = "Server=localhost;Database=pms;Uid=root;Pwd=1234";
                    MySqlConnection connection = new MySqlConnection();
                    connection.ConnectionString = ConnectionString;
                    Console.WriteLine(connection.Ping().ToString());
                    connection.Open();
                    Console.WriteLine("Connection to the database is established.");

                    //HardCoding.......
                    string queryString = "Insert Into parking_space(idParking_Space,Floor,Parking_Slot,Status) Values('" + idParking_Space + "','" + Floor + "','" + Parking_Slot + "','" + Status + "')";

                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = queryString;
                    command.Connection = connection;

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                    {
                        Console.WriteLine("Data Entered.");
                    }
                    connection.Close();

                    //This Should Not Be Done.... Instead Collision Avoidance should be added here.....
                    idParking_Space++;
                    Parking_Slot++;

                }
                else
                {
                    Console.WriteLine("Data Not Entered.");
                }
            }
            
            
        }
    }
}
