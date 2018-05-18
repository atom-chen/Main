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
		ssFileContent.getline(szOnLine, 256);      //�ó�������һ�У��ŵ�szOnLine��
		if (strlen(szOnLine) > 0)                 //������в�Ϊ��
		{
			if (szOnLine[0] == 'v')                  
			{
				if (szOnLine[1] == 't')              //�����vt
				{
					printf("textcoord:%s\n",szOnLine);
				}
				else if (szOnLine[1] == 'n')         //�����vn
				{
					printf("normal:%s\n", szOnLine);
				}
				else                               //�����v��ͷ
				{
					printf("position:%s\n", szOnLine);
					
				}
			}
			else if (szOnLine[0] == 'f')             //�����f��ͷ
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