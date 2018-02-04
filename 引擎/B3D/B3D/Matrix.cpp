#include "Matrix.h"
namespace B3D
{
	bool Matrix44::operator == (const Matrix44& matrix) const      //判断是否相等
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
	Matrix44 Matrix44::operator + (const Matrix44& matrix)        //矩阵加法
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
	Matrix44 Matrix44::operator - (const Matrix44& matrix)        //矩阵减法
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
	Matrix44 Matrix44::operator*(const float& n)                 //矩阵数乘
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
	Matrix44 Matrix44::operator*(const Matrix44& matrix)        //矩阵乘法
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
	void Matrix44::TransPoseIn()                             //将源矩阵转置
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
	void Matrix44::Identity()                                      //将该矩阵置为单位矩阵
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
			//第i行*第j列
			temp += matrix1.m_Matrix[i][k] * matrix2.m_Matrix[k][j];
		}
		return temp;
	}

	Matrix44  Matrix44::TransPose()                             //拿到源矩阵的转置矩阵（但不转置源矩阵）
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
