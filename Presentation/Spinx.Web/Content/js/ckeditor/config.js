/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
    
    config.extraPlugins = "autogrow";
    config.autoGrow_onStartup = true;

    config.allowedContent = true;
    config.FormatSource = false;
    config.fillEmptyBlocks = false;
    config.enterMode = CKEDITOR.ENTER_P;

    config.contentsCss =
    [
        //virtualDir + "Content/Admin/css/ckstyle.css",
        //virtualDir + "Content/css/reset.css", 
        //virtualDir + "Content/css/style.css",
        //virtualDir + "Content/css/media.css", 
        //"https://fonts.googleapis.com/css?family=Muli:300,400,400i,600,700,800,900"
    ];

    config.filebrowserBrowseUrl =  'js/ckfinder/ckfinder.html';

    $.each(CKEDITOR.dtd.$removeEmpty, function (i) {
        CKEDITOR.dtd.$removeEmpty[i] = false;
    });
    CKEDITOR.dtd.$removeEmpty['a'] = false;
};

//CKEDITOR.scriptLoader.load(["/Content/js/jquery.min.js", "/Content/js/easyResponsiveTabs.js", "/Content/js/custom.js"]);
