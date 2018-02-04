#pragma once
#include "B3DClass.h"

#define NULLMATEIX Matrix44();
#define IDENTITYMATEIX Matrix44(1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1);
namespace B3D
{
	class Matrix44
	{
		using Element= float[16];
	private:
		float m_Matrix[4][4];
	public:       //����/�������
		Matrix44()
		{
			memset(m_Matrix, 0, sizeof(float) * 16);
		}
		Matrix44(const Matrix44& matrix)             //ֵ����
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					m_Matrix[i][j] = matrix.m_Matrix[i][j];
				}
			}
		}    
		Matrix44(Element elements)
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					m_Matrix[i][j] = elements[i + j];
				}
			}
		}
		Matrix44(float a11, float a12, float a13, float a14,
			float a21, float a22, float a23, float a24,
			float a31, float a32, float a33, float a34,
			float a41, float a42, float a43, float a44)
		{
			m_Matrix[0][0] = a11;
			m_Matrix[0][1] = a12;
			m_Matrix[0][2] = a13;
			m_Matrix[0][3] = a14;

			m_Matrix[1][0] = a21;
			m_Matrix[1][1] = a22;
			m_Matrix[1][2] = a23;
			m_Matrix[1][3] = a24;

			m_Matrix[2][0] = a31;
			m_Matrix[2][1] = a32;
			m_Matrix[2][2] = a33;
			m_Matrix[2][3] = a34;

			m_Matrix[3][0] = a41;
			m_Matrix[3][1] = a42;
			m_Matrix[3][2] = a43;
			m_Matrix[3][3] = a44;
		}
		Matrix44& operator=(const Matrix44& matrix)     //ֵ����
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					m_Matrix[i][j] = matrix.m_Matrix[i][j];
				}
			}
			return *this;
		}
	public:
		//�������㺯��
		bool operator==(const Matrix44& matrix) const;      //�ж��Ƿ����
		Matrix44 operator+(const Matrix44& matrix);        //����ӷ�
		Matrix44 operator-(const Matrix44& matrix);        //�������
		Matrix44 operator*(const float& n);                 //��������
		Matrix44 operator*(const Matrix44& matrix);        //����˷�
		void TransPoseIn();                             //��Դ����ת��
		void Identity();                                //���þ�����Ϊ��λ����
	public:        //���ܺ���	
		inline float& at(const size_t& i,const size_t& j) const;
		

	public:        //���ߺ���
		Matrix44  TransPose();                             //�õ�Դ�����ת�þ��󣨵���ת��Դ����
		static Matrix44  GetIdentity(){ return IDENTITYMATEIX };
	private:       //�����ù��ߺ���
		inline float LineiMultiplyRowj(const Matrix44& matrix1, const Matrix44& matrix2, unsigned i, unsigned j);
		inline bool IsEmpty() const;
		
	};


}