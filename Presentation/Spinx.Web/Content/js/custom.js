
$(document).ready(function() {
	/*** Header fixed ***/
	$(window).scroll(function () {
		scroll_pixel = $(window).scrollTop();
		((scroll_pixel < 100) ? $('header').removeClass('header-fix') : $('header').addClass('header-fix'));
	});
	/*** for Mobile header nav ***/
	headerresize();
});

function headerresize(){
	/*** reset div ***/
	$('header').removeClass('navbar-close');
	$('body').removeClass('open-navbar');
	$('.nav-brand').hide();
	$('.nav-brand > ul > li.parent > a').removeClass('open-subnav');
	$('.submenu-wrap').show();
	
	
	/*** Header nav toggle for responsive   ***/
	$('.navbar-toggle').off('click');
	$('.navbar-toggle').on('click',function(){			
		$('header').toggleClass('navbar-close');
		$('body').toggleClass('open-navbar');
		$('.nav-brand').slideToggle(200,function(){
			if(!$('header').hasClass('navbar-close'))	
			{
				$('.nav-brand > ul > li.parent').find('> .submenu-wrap').hide();
				$('.nav-brand > ul > li.parent').find('a').removeClass('open-subnav');
			}			
		});
	});

	var screen_width = $( window ).width();
	if(screen_width < 767) {		
		$('.submenu-wrap').hide();
		$('.nav-brand > ul > li.parent > a').off('click');
		$('.nav-brand > ul > li.parent > a').on('click',function(event){
			event.preventDefault();
			$(this).closest(".nav-brand > ul > li.parent").find('> .submenu-wrap').slideToggle('slow');
			$(this).toggleClass('open-subnav');
		});
		
		$('li.has-submenu .submenu-nav').hide();
		$('.nav-brand li.parent li.has-submenu a').off('click');
		$('.nav-brand li.parent li.has-submenu a').on('click',function(event){
			event.preventDefault();
			$(this).closest(".nav-brand li.parent li.has-submenu").find('.submenu-nav').slideToggle('slow');
			$(this).parent().toggleClass('open-second-subnav');
		});
		
		/*** To move top-bar to the nav ***/
		$('.top-bar ul').insertAfter('.nav-brand > ul');
		$('.nav-brand > ul#nav > li.nav-search').insertBefore('.nav-brand > ul#nav > li:first-child');
	}
	else
	{
		/*** for Desktop ***/
		$('.nav-brand').show();
		$('header').removeClass("navbar-close");
		$('body').removeClass("open-navbar");
		$('.submenu-wrap').show();
	
		$('.nav-brand').find('#topbar').appendTo('.top-bar');
		$('.nav-brand > ul#nav').find('li.nav-search').appendTo('.nav-brand > ul#nav');
	}
};

$(window).resize(function(){
	headerresize();
});