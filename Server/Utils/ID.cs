using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Server.Utils {
    public class ID {
        private static readonly object lockObj = new();
        private static readonly long epoch = new DateTime(2023, 1, 1).Ticks;
        private static long lastTimestamp = -1L;
        private static long sequence = 0;
        private static long machineId;

        private static long GenerateMachineId() {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            string macAddress = networkInterfaces[0].GetPhysicalAddress().ToString();

            byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(macAddress));

            return BitConverter.ToInt64(hash, 0);
        }

        public ID() {
            machineId = GenerateMachineId();
        }

        private static long WaitNextMillis(long lastTimestamp) {
            long currentTimestamp = (DateTime.UtcNow.Ticks - epoch) / 10000;

            while (currentTimestamp <= lastTimestamp) {
                currentTimestamp = (DateTime.UtcNow.Ticks - epoch) / 10000;
            }

            return currentTimestamp;
        }

        public static long GenerateID() {
            lock (lockObj) {
                long timestamp = (DateTime.UtcNow.Ticks - epoch) / 10000;

                if (timestamp == lastTimestamp) {
                    sequence = (sequence + 1) & 4095;

                    if (sequence == 0) {
                        timestamp = WaitNextMillis(lastTimestamp);
                    }
                } else {
                    sequence = 0;
                }

                lastTimestamp = timestamp;

                long snowflakeId = (timestamp << 22) | (machineId << 12) | sequence;

                return snowflakeId;
            }
        }
    }
}
