#include "absClass.h"
#include "Enemy.h"
#include "Player.h"

int main()
{
	AbsClass *abs = new Enemy;
	abs->Atack();
	delete abs;

	abs = new Player;
	abs->Atack();
	delete abs;
	return 0;
}