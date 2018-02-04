using namespace std;

//×Óµ¯Àà
class SwordBullets{
private:
	unsigned m_X, m_Y;
	bool m_Exist;
public:
	void SetX(unsigned x){ this->m_X = x; }
	void SetY(unsigned y){ this->m_Y = y; }
	void SetExist(bool exist){ this->m_Exist =exist ; }
	const unsigned& X()const { return m_X; }
	const unsigned& Y()const { return m_Y; }
	const bool& Exist()const { return m_Exist; }
};