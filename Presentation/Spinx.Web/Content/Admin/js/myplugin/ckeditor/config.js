/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    
    //config.extraPlugins = "autogrow,jsplus_backup,codemirror,autolink,autolinkmail";
    config.extraPlugins = "autogrow,jsplus_backup,codemirror,clipboard,autolink";
    config.autoGrow_onStartup = true;

    config.allowedContent = true;
    config.FormatSource = false;
    config.fillEmptyBlocks = false;
    config.enterMode = CKEDITOR.ENTER_P;

    config.baseHref = virtualDir;

    config.contentsCss =
    [
        virtualDir + "Content/Admin/css/ckstyle.css",
        virtualDir + "Content/css/reset.css", 
        //virtualDir + "Content/css/nav-search.css", 
        virtualDir + "Content/css/style.css",
        virtualDir + "Content/css/media.css",
        virtualDir + "Content/css/fonts.css"
    ];

    config.filebrowserBrowseUrl = virtualDir + 'ckfinder/ckfinder.html';

    $.each(CKEDITOR.dtd.$removeEmpty, function (i) {
        CKEDITOR.dtd.$removeEmpty[i] = false;
    });
    CKEDITOR.dtd.$removeEmpty['a'] = false;

    config.codemirror = {
        autoFormatOnStart: true
    };
};

//CKEDITOR.scriptLoader.load(["/Content/js/jquery.min.js", "/Content/js/easyResponsiveTabs.js", "/Content/js/custom.js"]);