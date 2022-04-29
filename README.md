# emgucv-pow-mul-bug

Exhibits a weird behavior in [Emgu.CV](https://github.com/emgucv/emgucv):

When targeting .NET 4.8/OpenCL and running on the GPU, `CvInvoke.Pow(umat, 2, destination)` gives results different from `CvInvoke.Multiply(umat, umat, destination)`.

The different results are probably due to the GPU rounding behavior, however this difference does not appear:

* When running on the CPU
* When targeting .NET 6 (be it on the CPU or the GPU)

The values with .NET 6 are the same as .NET 4/CPU. .NET 4/GPU is the only outlier.

In order to witness the difference, build the example project with `net48` target, then run it once without arguments (will generate the GPU results) and another time passing it `--noopencl`.

Compare the two generated files: they will be different.

PS: I witness all of this on a machine with an Intel UHD Graphics 3.0 card (driver version: 30.0.101.1003)
