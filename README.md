# emgucv-pow-mul-bug

Exhibits a weird behavior in [Emgu.CV](https://github.com/emgucv/emgucv):

When targeting .NET 4.8/OpenCL and running on the GPU, `CvInvoke.Pow(umat, 2, destination)` gives results different from `CvInvoke.Multiply(umat, umat, destination)`.

The different results are probably due to the GPU rounding behavior, however this difference does not appear:

* When running on the CPU
* When targeting .NET 6 (be it on the CPU or the GPU)
* When targeting .NET 4.8 but using the NVidia GPU

The values with .NET 6 are the same as .NET 4/CPU. .NET 4/Intel GPU is the only outlier.

To sum it up, the weird behavior only appears when using .NET 4.8 and the Intel GPU. In all other cases (CPU vs GPU, NVidia vs Intel or .NET 6 vs .NET 4.8), the results are the same.

## Usage

In order to witness the difference, build the example project with `net48` target, then run it once without arguments (will generate the GPU results) and another time passing it `--noopencl`.

Compare the two generated files: they will be different.

PS: I witness all of this on a machine with an Intel UHD Graphics 3.0 card (driver version: 30.0.101.1003)
