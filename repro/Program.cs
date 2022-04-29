using System;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Ocl;
using Emgu.CV.Structure;

namespace NormalizeTest4
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var enableOpenCL = !args.Select(a => a.ToLowerInvariant()).Contains("--noopencl");
            //if (enableOpenCL)
            //    Utils.SetOpenCLDeviceConfiguration("Intel:GPU:");
            Utils.EnableOpenCL(enableOpenCL);

            Console.WriteLine("Configuration:");
            Console.WriteLine($"  Architecture: {(IntPtr.Size == 8 ? "x64" : "x86")}");
            Console.WriteLine($"  OpenCL GPU      : {CvInvoke.HaveOpenCLCompatibleGpuDevice}");
            Console.WriteLine($"  OpenCL Support  : {CvInvoke.HaveOpenCL}");
            Console.WriteLine($"  OpenCL Activated: {CvInvoke.UseOpenCL}");

            if (enableOpenCL)
                Console.WriteLine($"Current default OpenCL Device is: {Device.Default.Name}");

#if NETFRAMEWORK
            var fx = "net48";
#else
            var fx = "net60";
#endif

            var kind = enableOpenCL ? "ocl" : "cpu";
            var gpu = enableOpenCL ? Device.Default.VendorName.Split(' ')[0].ToLowerInvariant().Replace("(r)", "") : "none";

            var inputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "u.png");
            using (var input = new Image<Gray, byte>(inputFile))
            using (var temp = input.Convert<Gray, float>())
            using (var floats = temp.ToUMat())
            using (var pow = new UMat())
            using (var mul = new UMat())
            {
                CvInvoke.Pow(floats, 2, pow);
                CvInvoke.Multiply(floats, floats, mul);

                Save(pow, $@"c:\temp\pow-{fx}-{kind}-{gpu}.bin");
                Save(mul, $@"c:\temp\mul-{fx}-{kind}-{gpu}.bin");
            }
        }

        private static void Save(UMat umat, string filename)
        {
            using (var file = File.OpenWrite(filename))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write(umat.Size.Width);
                writer.Write(umat.Size.Height);
                writer.Write(umat.Bytes.Length);
                writer.Write(umat.Bytes);
            }
        }
    }
}
