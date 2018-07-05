
//递归解析字符串算法
template<typename _arg,typename... _args>
void tsnprintf(char* buf,int size,const char* format,_args value,_args values)
{
	if(buf==nullptr || size<=0 || format==nullptr)
	{
		return;
	}
	char* const buf_start = buf;                         //本次调用起始位置
	char* const buf_end = buf_start + size - 1;             //本次调用终止位置

	char* buf_it = buf_start;                          //当前输出位置
	const char* format_it = format;                    //当前解析位置

	while((buf_it<buf_end) && (format_it!=NULL))
	{
		if(*format_it == '%')
		{   
			//如果是两个% 则说明用户想要转义为%
			if(format_it[1]=='%')
			{
			    *buf_it++='%';
			    format_it += 2;			
			}
			else
			{
				int remain_size=static_cast<int>(buf_end - buf_it + 1);     //buf剩余长度=buf end位置-buf当前位置
				int value_length=value_to_string(buf_it,remain_size,value);
				buf_it += max(min(value_length,remain_size-1),0);
				format_it++;
				remain_size=static_cast<int>(buf_end - buf_it + 1);
				return tsnprintf(buf_it,remain_size,format_it,values...);
			}

		}
		else
		{
			*buf_it++ = *format_it++;
		}
	}


	if(buf_it<buf_end)
	{
		*buf_it='\0';
	}
	*buf_end='\0';

}