[![Build Status](https://travis-ci.org/Excolo/TerrainQuest.svg?branch=master)](https://travis-ci.org/Excolo/TerrainQuest)

TerrainQuest
=====
A simple C# based terrain generation / rendering tool

Allows you to build a terrain generation graph structure, where each node performs some process such as noise generation, erosion, stamping, image effects, etc. Nodes are structured as a dependency graph, which means that you can use special blending nodes to combine the result of other nodes, such as heightmap generation nodes. 

The graph structure is fully serializable so the entire terrain generation graph can be saved to a file and reloaded upon request. 

Features
-----
#### Terrain generation
* Graph structure to combine multiple generation processes
* Serialization of graph
* Mix and Mask blending nodes to combine result of other nodes

Planned features
-----

#### Terrain generation
* More types of random algorithms
* Hydraulic erosion

#### Rendering
* Efficient terrain mesh structure
* Better shading
