# AnytimeAStar
Anytime A Star (ARA*)

This variant of A* starts with a weighted A*, generating a non-optimal path, but faster than the vanilla A* initially. Then progressibely optimising the path after a few iterations.

This implementation, does the planning in a seperate thread, allowing the rendering to happen in the main thread.

https://user-images.githubusercontent.com/19212519/206883244-b7bfd642-e2b3-4e8d-aa6d-219cc6523a7f.mov

Based on:
The python implementation: https://github.com/zhm-real/PathPlanning
Paper: ARA*: Anytime A* with Provable Bounds on Sub-Optimality 
https://proceedings.neurips.cc/paper/2003/file/ee8fe9093fbbb687bef15a38facc44d2-Paper.pdf
