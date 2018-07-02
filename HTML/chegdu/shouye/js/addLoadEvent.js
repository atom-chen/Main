/*下面是addLoadEvent函数将要完成的操作*
 * #把现有的window.onload事件处理函数的值存入变量onload
 * #如果在这个处理函数上还没有绑定任何函数，就像平时那样吧新函数添加给它
 * #如果在这个处理函数上已经绑定了一些函数，就把新函数追加到现有指令的末尾
 */
function addLoadEvent(func){
	var oldonload = window.onload;
	if(typeof window.onload != 'function'){
		window.onload = func;
	}else{
		window.onload = function(){
			oldonload();
			func();
		}
	}
}

