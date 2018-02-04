#include "Matrix.h"
namespace B3D
{
	bool Matrix44::operator == (const Matrix44& matrix) const      //�ж��Ƿ����
	{
		if (this->IsEmpty() && matrix.IsEmpty())
		{
			return 1;
		}
		if (this->IsEmpty())
		{
			return 0;
		}
		if (matrix.IsEmpty())
		{
			return 0;
		}
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (this->m_Matrix[i][j] != matrix.m_Matrix[i][j])
				{
					return 0;
				}
			}
		}
		return true;
	}
	Matrix44 Matrix44::operator + (const Matrix44& matrix)        //����ӷ�
	{
		if (this->IsEmpty())
		{
			return NULLMATEIX;
		}
		if (matrix.IsEmpty())
		{
			return NULLMATEIX;
		}
		Matrix44 temp;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				temp.m_Matrix[i][j] = this->m_Matrix[i][j] + matrix.at(i, j);
			}
		}
		return temp;
	}
	Matrix44 Matrix44::operator - (const Matrix44& matrix)        //�������
	{
		if (this->IsEmpty())
		{
			return NULLMATEIX;
		}
		if (matrix.IsEmpty())
		{
			return NULLMATEIX;
		}
		Matrix44 temp;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				temp.m_Matrix[i][j]=m_Matrix[i][j] - matrix.m_Matrix[i][j];
			}
		}
		return temp;
		
	}
	Matrix44 Matrix44::operator*(const float& n)                 //��������
	{
		if (this->IsEmpty())
		{
			return NULLMATEIX;
		}
		Matrix44 temp;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				temp.m_Matrix[i][j]=this->m_Matrix[i][j] * n;
			}
		}
		return temp;
	}
	Matrix44 Matrix44::operator*(const Matrix44& matrix)        //����˷�
	{
		if (matrix.IsEmpty() || this->IsEmpty())
		{
			return NULLMATEIX;
		}
		Matrix44 temp;
		for (unsigned int i = 0; i < 4; i++)
		{
			for (unsigned int j = 0; j < 4; j++)
			{
				temp.m_Matrix[i][j] = LineiMultiplyRowj(*this, matrix, i, j);
			}
		}
		return temp;
	}
	void Matrix44::TransPoseIn()                             //��Դ����ת��
	{
		if (this->IsEmpty())
		{
			return;
		}
		Matrix44 temp = *this;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				this->m_Matrix[i][j] = temp.m_Matrix[j][i];
			}
		}
	}
	void Matrix44::Identity()                                      //���þ�����Ϊ��λ����
	{
		if (this->IsEmpty())
		{
			return;
		}
		(*this) = IDENTITYMATEIX;
	}
	bool Matrix44::IsEmpty() const
	{
		return this->m_Matrix == nullptr;
	}
	float Matrix44::LineiMultiplyRowj(const Matrix44& matrix1, const Matrix44& matrix2, unsigned i, unsigned j)
	{
		if (i >= 4 || j >= 4)
		{
			return NULL;
		}
		if (matrix1.IsEmpty() || matrix2.IsEmpty())
		{
			return NULL;
		}
		float temp = 0;
		for (unsigned int k = 0; i < 4; i++)
		{
			//��i��*��j��
			temp += matrix1.m_Matrix[i][k] * matrix2.m_Matrix[k][j];
		}
		return temp;
	}

	Matrix44  Matrix44::TransPose()                             //�õ�Դ�����ת�þ��󣨵���ת��Դ����
	{
		if (this->IsEmpty())
		{
			return NULLMATEIX;
		}
		Matrix44 temp = *this;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				temp.m_Matrix[i][j] =this->m_Matrix[j][i];
			}
		}
		return temp;
	}

}
