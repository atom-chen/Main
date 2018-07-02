$(document).ready(function(){
//	$(".introduce").animate({width:"1000px",height:"15px",opacity:1},{duration:1000});
//	$(".c1,.h1").fadeIn(2000,function(){
//		$(".c1,.h1").fadeOut(2000,function(){
//			$(".c2,.h2").fadeIn(2000,function(){
//				$(".c2,.h2").fadeOut(2000
////					,function(){
////					$(".c3,.h3").fadeIn(2000,function(){
////						$(".c3,.h3").fadeOut(2000,function(){
////			
////		});
//				);
//			});
//		});
//		});
//	$(".cd_in,.hn_in").fadeIn(2000,function(){
//		$("a").fadeIn(2000);
//	});
		
			var $index = 3;
			
			setInterval(function(){
				$index--;
				if($index<1){
					$index = 3;
					$("#cd img").fadeIn(1000);
				}
				$("#cd img").eq($index).fadeOut(1000);
				$index = $index;
			},1000);
})
