# PerformanceMiddleware
A middleware that adjusts and monitors thread pool sizes for services running on Kubernetes.

The value set in the environment variable DOTNET_PROCESSOR_COUNT is established as the respective values of ThreadPool.GetMinThreads, hence the separate creation of this class.