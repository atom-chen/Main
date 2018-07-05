

void StartUp(const char* 项目名称,int16 世界id,const char* 进程名称,bool 是否使用程序名称作为日志文件名的前缀,bool 是否对特殊日志添加占位信息，以使日志文件必然存在,
	          响应命令的回调函数,确认某个逻辑线程是否有效的回调函数,枚举类型 当前时区,bool 是否按小时切分日志,bool 是否使用自定义的日志输出路径,const char* 日志输出路径)