#include<iostream>
#include<string>
#include<vector>
#include"State.h";
using namespace std;

//×´Ì¬»úÄ£°å
template<class T>
class StateMachine{
public:
	StateMachine(T *onwer) :m_pOnwer(onwer);
public:
	void Update();
	void ChangeState(State<T> *newState);
	void RevertToPreviousState();
public:
	void setCurrentState(State<T> *CurrentState)
	{
		this->m_pCurrentState = CurrentState;
	}
	void setGlovalState(State *GlovalState)
	{
		this->m_pGlovalState = GlovalState;
	}
private:
	State<T> *m_pCurrentState;
	State<T> *m_pPreviousState;
	State<T> *m_pGlovalState;
	T *m_pOnwer;
};