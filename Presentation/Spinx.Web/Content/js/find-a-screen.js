$(document).ready(function() {
	/*** Find A Screen - Multistep ***/
	var current_fs, next_fs, reset_fs;
	var marginLeft;
	var animating;
	StepProgress();
	
	$(".next").click(function(){
		if(animating) return false;
		animating = true;

		current_fs = $(this).closest('.screen-step');
		next_fs = $(this).closest('.screen-step').next();		
		
		current_fs.removeClass('prev-step');
		next_fs.show().addClass('active-step');
		current_fs.animate({marginLeft: -100 +"%"}, {
			step: function(now, mx) {
				marginLeft = (100 + now)+"%";
				current_fs.css({ 
					'position': 'absolute'
				});
				next_fs.css({'margin-left': marginLeft, 'position': ''});
			}, 
			duration: 800, 
			complete: function(){
				current_fs.hide();
				current_fs.addClass('prev-step');
				current_fs.removeClass('active-step');
				StepProgress();
				MoveTitle();
				StepCount();
				animating = false;
				/*** After ScrollTop ***/
				$('html, body').animate({
					scrollTop: $(".fs-title").offset().top - ($("header").height() + 10) + 'px'
				}, 800);
			}, 
			easing: 'easeInSine'
		});
	});
	
	/*** Restart Screen Finder ***/
	$(".reset-link a").click(function(){
		if(animating) return false;
		animating = true;

		current_fs = $(this).closest('.screen-step');
		reset_fs = $( "div.screen-step[data-item='1']" );

		reset_fs.show().addClass('active-step');
		current_fs.animate({marginLeft: -100 +"%"}, {
			step: function(now, mx) {
				marginLeft = (100 + now)+"%";
				current_fs.css({ 
					'position': 'absolute'
				});
				reset_fs.css({'margin-left': marginLeft, 'position': ''});
			}, 
			duration: 800, 
			complete: function(){
				current_fs.hide();
				current_fs.removeClass('active-step');
				StepProgress();
				MoveTitle();
				StepCount();
				animating = false;
				/*** After ScrollTop ***/
				$('html, body').animate({
					scrollTop: $(".fs-title").offset().top - ($("header").height() + 10) + 'px'
				}, 800);
			}, 
			easing: 'easeInSine'
		});
	});
	
	/*** Move Step Title to Banner Title ***/
	function MoveTitle(){
		var str = $( ".active-step h2" ).text();
		$( ".fs-title h2" ).html( str );
	}
	
	/*** Move Number of Steps to Banner Title ***/
	function StepCount(){
		var numItems = $('.screen-step').length;
		var activeItems = $('.active-step').attr( "data-item" ) + " of " + numItems;
		$( ".fs-title p" ).html( activeItems );
	}
	
	/*** Progressbar ***/
	function StepProgress(){
		var divideProgress = 100 / $('.screen-step').length;
		var total = $('.active-step').attr( "data-item" ) * divideProgress;
		$('#progressbar span').css({ 'width': total + "%"});
	}
	
	/*** Screen Step - 3: Custom dimensions ***/
	$('.custom-dimensions').hide();
	$("#SizeToggle").on('click',function(){			
		$(this).parent().addClass("show-dimensions");
		$(".custom-dimensions").slideToggle(200,function(){
			$('div#AspectRatio, #Dimensions').addClass('disabled');
		});
	});	
	$("#GoBack").on('click',function(){			
		$('#SizeToggle').parent().removeClass("show-dimensions");
		$(".custom-dimensions").slideToggle(200,function(){
			$('div#AspectRatio, #Dimensions').removeClass('disabled');
		});
	});

	
	$(".submit").click(function(){
		return false;
	})
	
});