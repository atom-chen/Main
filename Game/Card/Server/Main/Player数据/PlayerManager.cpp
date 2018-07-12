


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

//轮询socket状态
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

	timeval timev;
	timev.tv_sec=0;
	timev.tv_usec=0;
	int32_t nRet=select(static_cast<int32_t>(MaxSocketID+1),&m_ReadSet,&m_WriteSet,&m_ExceptSet,&timev);
}

//将状态异常的socket从集合中释放-》本次tick不做处理
void ProcessExceptions()
{
	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();
		if(FD_ISSET(s,&m_ExceptSet))
		{
			it=Del(ptr,PlayerOpType::DELPLAYER_EXP);
			continue;
		}
		it++:
	}
}

/*
以下函数在处理成功/处理失败后将player移出playerList，并标注删除的原因，在删除后会调用函数OnDelPlayer(PlayerPtr ptr,int32_t nResult)处理离开事件
*/
void ProcessInputs()
{
	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();
		if(FD_ISSET(s,&m_ReadSet))
		{
			//检查socket当前状态
			if(ptr->GetSocket().isSocketError())
			{
				it=Del(ptr,PlayerOpType::DELPLAYER_INPUTEXP1);
				continue;			
			}
			//处理消息
			else
			{
				bool bRet=ptr->ProcessInput();
				if(!bRet)
				{
				    it=Del(ptr,PlayerOpType::DELPLAYER_INPUTEXP2);            //消息处理失败
				    continue;
				}
				it=Del(ptr,PlayerOpType::DELPLAYER_INPUTEXP3);               //消息处理成功
				continue;
			}
		}
		it++:
	}	
}
void ProcessCommands()
{
	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();
		//检查socket当前状态
		if(ptr->GetSocket().isSocketError())
		{
			it=Del(ptr,PlayerOpType::DELPLAYER_COMMANDEXP1);
			continue;			
		}
		//处理命令
		else
		{
			bool bRet=ptr->ProcessCommand();
			if(!bRet)
			{
				it=Del(ptr,PlayerOpType::DELPLAYER_COMMANDEXP2);            //处理失败
				continue;
			}
			it=Del(ptr,PlayerOpType::DELPLAYER_COMMANDEXP3);               //处理成功
			continue;
		}
	}
	it++:
}
void ProcessOutputs()
{
	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();
		if(FD_ISSET(s,&m_WriteSet))
		{
			//检查socket当前状态
			if(ptr->GetSocket().isSocketError())
			{
				it=Del(ptr,PlayerOpType::DELPLAYER_OUTPUTEXP1);
				continue;			
			}
			//处理输出
			else
			{
				bool bRet=ptr->ProcessOutput();
				if(!bRet)
				{
				    it=Del(ptr,PlayerOpType::DELPLAYER_OUTPUTEXP2);            //处理失败
				    continue;
				}
				it=Del(ptr,PlayerOpType::DELPLAYER_OUTPUTEXP3);               //处理成功
				continue;
			}
		}
		it++:
	}	
}

//处理Update
void ProcessTicks(const sol_routine_time &rt)
{
	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		bool bRet=ptr->Tick(rt);         //让该player处理逻辑更新
		if(!bRet)
		{
			it=Del(ptr,PlayerOpType::DELPLAYER_TICK1);
			continue;
		}
		it=Del(ptr,PlayerOpType::DELPLAYER_TICK2);
		continue;
		it++;
	}	
}

//是否发送心跳包
void ProcessHeartbeat(const sol_routine_time &rt)
{
	m_nProcessHeartbeatTime+=Time.delta;                  //时间++
	if(累计时间不够心跳频率)
	{
		return;
	}
	for(auto it=playerList.begin();it!=playerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();
		//检查socket当前状态
		if(ptr->GetSocket().isSocketError())
		{
			it=Del(ptr,PlayerOpType::DELPLAYER_HEARTBEATEXP1);
			continue;			
		}
		//处理心跳
		else
		{
			bool bRet=ptr->ProcessHeartbeat();
			if(!bRet)
			{
				it=Del(ptr,PlayerOpType::DELPLAYER_HEARTBEATEXP2);            //处理失败
				continue;
			}
			it=Del(ptr,PlayerOpType::DELPLAYER_HEARTBEATEXP3);               //处理成功
			continue;
		}
		it++:
	}	
}

//查询每个connectsocket的状态
void QueryConnectingSockets()
{
	SOCKET MaxSocketID=m_ConnectPlayerList.front()->GetSocket().getSOCKET();

	FD_ZERO(&m_ConnectingReadSet);
	FD_ZERO(&m_ConnectingWriteSet);
	FD_ZERO(&m_ConnectingExceptSet);

	for(auto it=m_ConnectPlayerList.begin();it!=m_ConnectPlayerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();

		FD_SET(s,&m_ConnectingReadSet);
		FD_SET(s,&m_ConnectingWriteSet);
		FD_SET(s,&m_ConnectingExceptSet);
		if(MaxSocketID<s)
		{
			MaxSocketID=s;
		}
		it++;
	}
	timeval timev;
	timev.tv_sec=0;
	timev.tv_usec=0;
	int32_t nRet=select(static_cast<int32_t>(MaxSocketID+1),&m_ReadSet,&m_WriteSet,&m_ExceptSet,&timev);	
}

//处理连接
void ProcessConnecting()
{
	for(auto it=m_ConnectPlayerList.begin();it!=m_ConnectPlayerList.end();)
	{
		PlayerPtr ptr=(*it);
		SOCKET s=ptr->GetSocket().getSOCKET();

		if(FD_ISSET(s,&m_ConnectingReadSet) || FD_ISSET(s,&m_ConnectingWriteSet))           //如果s在读写事件set中，则将其添加到player集合
		{
			if(ptr->GetSocket().isSocketError())
			{
				it=DelConnecting(ptr,ConnectingOpType::DELCONNECTING_SOCKETERROR);            //将发生错误的连接断开
				continue;       
			}
			else
			{
				it=DelConnecting(ptr,ConnectingOpType::DELCONNECTING_CONNECTED);
				ptr->SetStatus(ServerPlayerStatus::CONNECTED);
				Add(ptr,PlayerOpType::ADDPLAYER_CONNECTINGREADY);                               //将正常的玩家加入到playerList，等待下次tick处理其Update     
				continue;   
			}
			it=DelConnecting(ptr,ConnectingOpType::DELCONNECTING_SOCKETEXCEPTION);            //socket异常，删除
			continue;
		}
		if(FD_ISSET(s,&m_ConnectingExceptSet))                                               //如果s在异常set中，认为select发现异常，删除
		{
			it=DelConnecting(ptr,ConnectingOpType::DELCONNECTING_SELECTEXCEPTION);           
			continue;		
		}
		it++;
	}	
}


/*--------------------------------------------------------------子类-----------------------------------------------------------------*/
void LoginPlayerManager::OnAddPlayer(PlayerPtr ptr,int32_t nResult)
{
	PlayerManager::OnAddPlayer(ptr,nResult);

	ClientPlayerPtr cliptr=boost::dynamic_pointer_cast<ClientPlayer,Player>(ptr);
	cliptr->GetPlayerLoginData().SetLoginRoutine(&m_rLoginRoutine);

	m_rLoginRoutine.SetCurPlayerCount(static_cast<int32_t>(m_PlayerList.size()));

	//..日志
}
void LoginPlayerManager::OnDelPlayer(PlayerPtr ptr,int32_t nResult)
{
	PlayerManager::OnDelPlayer(ptr,nResult);

	ClientPlayerPtr cliptr=boost::dynamic_pointer_cast<ClientPlayer,Player>(ptr);
	cliptr->GetPlayerLoginData().SetLoginRoutine(nullptr);

	m_rLoginRoutine.SetCurPlayerCount(static_cast<int32_t>(m_PlayerList.size()));

	switch(cliptr->GetStatus())
	{
		//...各种case
	}
	
	//...日志
}	