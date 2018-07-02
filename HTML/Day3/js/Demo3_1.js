//实现函数
function playPause(img)
{
	//获取音乐
	var music=document.getElementById("musicplayer");
	if(music.paused)
	{
		//播放音乐
		music.play();
		img.src="img/music.png";		
	}
	else
	{
		//暂停音乐
		music.pause();
		img.src="img/stop.png"	
	}
}
