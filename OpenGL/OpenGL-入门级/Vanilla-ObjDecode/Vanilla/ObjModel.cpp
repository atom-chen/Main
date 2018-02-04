/************************************************************************/
/* 
v -0.500000 -0.500000 0.500000
v 0.500000 -0.500000 0.500000
v -0.500000 0.500000 0.500000
v 0.500000 0.500000 0.500000
v -0.500000 0.500000 -0.500000
v 0.500000 0.500000 -0.500000
v -0.500000 -0.500000 -0.500000
v 0.500000 -0.500000 -0.500000                                                                     */
/************************************************************************/

#include "ObjModel.h"
#include "utils.h"
#include <string>
#include <sstream>
void ObjModel::Init(const char* ObjModel)
{
	struct FloatData
	{
		float v[3];
	};
	unsigned char* fileContent = LoadFileContent(ObjModel);
	std::stringstream ssFileContent((char *)fileContent);
	char szOnLine[256];
	while (!ssFileContent.eof())
	{
		memset(szOnLine, 0, 256);
		ssFileContent.getline(szOnLine, 256);      //拿出流的下一行，放到szOnLine中
		if (strlen(szOnLine) > 0)                 //如果该行不为空
		{
			if (szOnLine[0] == 'v')                  
			{
				if (szOnLine[1] == 't')              //如果是vt
				{
					printf("textcoord:%s\n",szOnLine);
				}
				else if (szOnLine[1] == 'n')         //如果是vn
				{
					printf("normal:%s\n", szOnLine);
				}
				else                               //如果以v开头
				{
					printf("position:%s\n", szOnLine);
					
				}
			}
			else if (szOnLine[0] == 'f')             //如果以f开头
			{
				printf("face%s\n", szOnLine);
			}
		}
	}


	delete fileContent;
}

void ObjModel::Draw()
{

}