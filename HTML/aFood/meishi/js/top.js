
 $(function () {
            var itemNum = $(".chengdu .chengdu_body .item").length;//要旋转的div的数量
            var itemDeg = 360 / itemNum;///计算平均偏移角度，后面的itemDeg*index是不同索引div的偏移角度
            $(".chengdu_body>.item").each(function (index, element) {
                $(element).css({
                    //给每一个item设置好位置
                    //rotateY让每一个item绕着Y轴偏移，itemDeg*index是不同索引div的偏移角度
                    //translateZ是控制item在角度偏移后，往他们的正上方移动的距离，数值越大旋转的范围越大
                    transform: "rotateY(" + itemDeg * index + "deg) translateZ(280px)"
                });
            });
        });
        
window.onload=function(){
	animates();
}

function animates(){
	$(".box .title p").animate({top:"0%"},{duration:1000,complete:function(){
		$(".chengdu_title").fadeIn(1000,function(){
			$(".chengdu_jianjie").fadeIn(2000,function(){
				$(".box .chengdu_body img").fadeIn(2000,function(){
					$(".chengdu_duilian_right").animate({right:"5%"},{duration:2000,complete:function(){
						$(".chengdu_duilian_left").animate({left:"5%"},{duration:2000});
					}});
				});
			});
		});
	}});
	
	$(window).scroll(function(){
		if ($(this).scrollTop()>200) {
			$(".hainan_title").fadeIn(1000,function(){
				$(".hainan_jianjie").fadeIn(1000,function(){
					$(".hainan_body img").fadeIn(2000,function(){
						$(".hainan_duilian_right").fadeIn(1000,function(){
							$(".hainan_duilian_left").fadeIn(1000);
						});
					})
				})
			})
		}
	})
}
