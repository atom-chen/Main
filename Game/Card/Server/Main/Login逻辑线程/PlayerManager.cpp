


void Tick(const sol_routine_time& rt)
{
	QuerySockets();
	ProcessExceptions();
	ProcessInputs();
	ProcessCommands();
	ProcessOutputs();

	ProcessTicks(rt);
	ProcessHeartbeat(rt);

	QueryConnectingSockets();
	ProcessConnecting();
}       

void QuerySockets()
{
	if(playerList.empty())
	{
		return;
	}
	SOCKET MaxSocketID=playerList.front()->GetSocket().getSOCKET();

	FD_ZERO(&m_ReadSet);
	FD_ZERO(&m_WriteSet);
	FD_ZERO(&m_ExceptSet);

	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();

		FD_SET(s,&m_ReadSet);
		FD_SET(s,&m_WriteSet);
		FD_SET(s,&m_ExceptSet);

		if(MaxSocketID<s)
		{
			MaxSocketID=s;
		}
		it++;
	}
}
void ProcessExceptions()
{

}
void ProcessInputs()
{

}
void ProcessCommands()
{

}
void ProcessOutputs()
{

}

void ProcessTicks(const sol_routine_time &rt)
{

}
void ProcessHeartbeat(const sol_routine_time &rt)
{

}

void QueryConnectingSockets()
{

}
void ProcessConnecting()
{

}