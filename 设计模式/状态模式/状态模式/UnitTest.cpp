#include "Content.h"
#include "GameState.h"

void main()
{
	Content *theContext = new Content();
	theContext->SetState(new GameState1(theContext));

	theContext->Requeat(5);
	theContext->Requeat(15);
	theContext->Requeat(25);
	theContext->Requeat(35);
}