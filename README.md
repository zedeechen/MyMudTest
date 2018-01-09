# MyMudTest

## TODO:
### 2018/1/9
- Scene/Map/Room 逻辑重写:
	* 各种移动/重定位（如传送）应基于Map对象。
	* Map对象包含一个或多个Room对象，一个Room可以被多个Map使用。（因此Room中不包含可以移动到哪里的信息）。
	* 由Map决定当前所处的Room、所属的Scene，以及可以向哪些方向移动、移动后的位置。
