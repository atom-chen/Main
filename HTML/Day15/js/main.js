//第一步：设置动画的画布
//初始相位，并创建一个900×420的游戏画布
//1、2参数，宽、高，相当于画布的大小
//3参数，渲染环境，可以使用的参数：Phaser.canvas/Phaser.webgl/Phaser.AUTO(推荐)
//4参数，为你想插入的相位器创建画布元素
var game = new Phaser.Game(900,420,Phaser.AUTO,"game_div");

//game_state是quick-cocos2d-x中特有的一个用户信息存储类
var game_state = {};
//记录跳跃机会
var x=2;
var choose=0;

//创建一个新的main的状态，将包含游戏
game_state.main = function(){};
//main原型
game_state.main.prototype = {
//	首先调用的函数：加载所有资源
	preload:function(){
		//	游戏背景 ,键值对
		this.game.load.image('bg','img/bkg.png');
		this.game.load.image('dog','img/dog.png');
		this.game.load.image('line','img/line.PNG');
		this.game.load.image('dogleft','img/dogleft.png');
		this.game.load.image('pipe','img/master.png');
		this.game.load.image('mas','img/wood.png');
		
		this.game.load.audio('game','mp3/game.mp3');
		this.game.load.audio('failed','mp3/failed.wav');
		this.game.load.audio('jump','mp3/jumpp.mp3');
	},
//	preload后调用的函数：设置游戏
	create:function(){
		this.game.add.sprite(0,0,'bg');
		this.dog=this.game.add.sprite(100,255,'dog');
		
		//给狗添加重力,使其能够下降（改变Y轴）
		this.dog.body.gravity.y=1000;
		//加入一个木板
		this.line=this.game.add.sprite(0,313,'line');
		
		//获取键盘的右箭头
		var right_key=this.game.input.keyboard.addKey(Phaser.Keyboard.RIGHT);
		//点击右键
		right_key.onDown.add(this.right,this);
		
		//获取键盘的左箭头
		var left_key=this.game.input.keyboard.addKey(Phaser.Keyboard.LEFT);
		left_key.onDown.add(this.left,this);
		
		//获取键盘的空格
		var space_key=this.game.input.keyboard.addKey(Phaser.Keyboard.SPACEBAR);
		space_key.onDown.add(this.jump,this);
		
		//出现障碍物
		//20个障碍物的组
		this.pipes=game.add.group();
		this.pipes.createMultiple(20,'pipe');
		
		this.pipe1=game.add.group();
		this.pipe1.createMultiple(20,'mas');
		//每隔1.5秒调用一次：出障碍物
		this.timer = this.game.time.events.loop(1000,this.add_row_of_pipes,this);
		//游戏音效
		this.failed=this.game.add.audio('failed');
		this.game_sound=this.game.add.audio('game');
		this.jump_sound=this.game.add.audio('jump');
		//游戏进行中音效
		this.game_sound.play();
		
		//在游戏左侧添加一个记分框
		this.score=0;
		var style={font:'30px Arial',fill:"#ffffff"};
		this.label_score=this.game.add.text(20,20,"0",style);
	},
	//这个函数每秒被调用60次→执行碰撞检测
	update:function()
	{
		//狗和line的碰撞
		this.game.physics.overlap(this.dog,this.line,this.land,null,this);
		//狗和障碍物1的碰撞
		this.game.physics.overlap(this.dog,this.pipes,this.restart_game,null,this);
		
		this.game.physics.overlap(this.dog,this.pipe1,this.restart_game,null,this);
		//出界死亡
		if(this.dog.inWorld==false)
		{
			this.restart_game();
		}
		
	},
	//狗和障碍物碰撞
	restart_game:function()
	{
		this.game_sound.stop();
		this.failed.play();
		//删除计时器
		this.game.time.events.remove(this.timer);
		//弹框：失败了
		alert("？？？？？");
		//重新开始游戏
		this.game.state.start('main');
	},
	//狗落到木板上，回弹的函数
	land:function()
	{
		//让狗向上跑
		this.dog.body.velocity.y=-150;
		x=2;
	},
	//向向右跑的函数
	right:function()
	{
		if(x<=900)
		{
			this.dog.body.velocity.x=150;
			this.dog.loadTexture('dog');
		}

	},
	//向左跑的函数
	left:function()
	{
		if(x>=0)
		{
			this.dog.body.velocity.x=-150;
			this.dog.loadTexture('dogleft');
		}
		
	},
	//跳的函数
	jump:function()
	{
		if(x>0)
		{
			this.jump_sound.play();
			this.dog.body.velocity.y=-320;
			x--;
		}
		
	},
	//将障碍物添加到桌面上
	add_one_pipe:function(x,y)
	{
		if(choose==0)
		{
			//获取障碍物到画布上
			var pipe=this.pipes.getFirstDead();
			pipe.reset(x,y);
			//每产生一个障碍物，就使其他障碍物位移，改变x轴
			pipe.body.velocity.x=-200;
			//障碍物移除屏幕后消失
			pipe.outOfBoundsKill=true;
			choose=1;
		}
		else{
			choose=0;
			//获取障碍物到画布上
			var pipe=this.pipe1.getFirstDead();
			pipe.reset(x,y);
			//每产生一个障碍物，就使其他障碍物位移，改变x轴
			pipe.body.velocity.x=-200;
			//障碍物移除屏幕后消失
			pipe.outOfBoundsKill=true;
		}
	},
	//出现6个障碍物
	add_row_of_pipes:function()
	{
		var hole=Math.floor(Math.random()*3);
		for(var i=0;i<2;i++)
		{
			if(i!=hole && i!=hole+1)
			{
				this.add_one_pipe((i+1)*800,280);
			}
		}
		//每次要显示障碍物时，分数++
		this.score++;
		this.label_score.content=this.score;
	}
}

//调用方法,显示所有东西
game.state.add('main',game_state.main);
game.state.start('main');

