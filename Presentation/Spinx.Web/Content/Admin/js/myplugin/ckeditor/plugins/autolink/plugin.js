/**
 * @license Copyright (c) 2003-2018, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
 */

(function () {
    'use strict';

    //var validEmailRegex = /(?:(<a[^>]*>(?:[^<]+)<\/a>))|((?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\]))|([\s\S])/gm;
    var regex = /(?:(<a[^>]*>(?:[^<]+)<\/a>))|((http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*))|([\s\S])/gm;

    CKEDITOR.plugins.add('autolink', {
        requires: 'clipboard',

        init: function (editor) {
            editor.on('paste', function (evt) {
                var data = evt.data.dataValue;

                if (evt.data.dataTransfer.getTransferType(editor) == CKEDITOR.DATA_TRANSFER_INTERNAL) {
                    return;
                }

                var m;
                var acc = '';
                while ((m = regex.exec(data)) !== null) {
                    if (typeof (m[1]) != 'undefined') {
                        //First group is defined: it will have a <a> tag
                        //So we just add it to the acumulator as-is.
                        acc += m[1];
                    }
                    else if (typeof (m[2]) != 'undefined') {
                        //Second group is defined: it will have an email
                        //we change it
                        //acc += '<a href="mailto:' + m[2] + '">' + m[2] + '</a>';
                        if (m[2].indexOf('@') !== -1) {
                            acc += '<a href="mailto:' + m[2] + '">' + m[2] + '</a>';
                        }
                        else {
                            if (m[2].indexOf('http') === 0 || m[2].indexOf('/') === 0) {
                                acc += '<a href="' + m[2] + '" target="_blank">' + m[2] + '</a>';
                            }
                            else {
                                acc += '<a href="//' + m[2] + '" target="_blank">' + m[2] + '</a>';
                            }
                        }
                    }
                    else {
                        //Any other character. We just add to the accumulator
                        acc += m[0];
                    }
                }
                data = acc;

                // If link was discovered, change the type to 'html'. This is important e.g. when pasting plain text in Chrome
                // where real type is correctly recognized.
                if (data != evt.data.dataValue) {
                    evt.data.type = 'html';
                }

                evt.data.dataValue = data;
            });
        }
    });
})();