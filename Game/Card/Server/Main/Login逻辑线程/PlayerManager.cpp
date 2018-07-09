


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