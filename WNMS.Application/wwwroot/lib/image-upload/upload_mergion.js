(function ($) {
    // 当domReady的时候开始初始化
    $(function () {
        var $wrap = $('#uploader'),

            // 图片容器
            $queue = $('<ul class="upload_box"></ul>')
                .appendTo($wrap.find('.queueList')),

            // 状态栏，包括进度和控制按钮
            $statusBar = $wrap.find('.statusBar'),

            // 文件总体选择信息。
            $info = $statusBar.find('.info'),

            // 上传按钮
            $upload = $wrap.find('.uploadBtn'),

            // 没选择文件之前的内容。
            $placeHolder = $wrap.find('.placeholder'),

            $progress = $statusBar.find('.progress').hide(),

            // 添加的文件数量
            fileCount = 0,

            // 添加的文件总大小
            fileSize = 0,

            // 优化retina, 在retina下这个值是2
            ratio = window.devicePixelRatio || 1,

            // 可能有pedding, ready, uploading, confirm, done.
            state = 'pedding',

            // 所有文件的进度信息，key为file id
            percentages = {},

            // 检测是否已经安装flash，检测flash的版本
            flashVersion = (function () {
                var version;
                try {
                    version = navigator.plugins['Shockwave Flash'];
                    version = version.description;
                } catch (ex) {
                    try {
                        version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash')
                            .GetVariable('$version');
                    } catch (ex2) {
                        version = '0.0';
                    }
                }
                version = version.match(/\d+/g);
                return parseFloat(version[0] + '.' + version[1], 10);
            })(),

            supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                        'WebkitTransition' in s ||
                        'MozTransition' in s ||
                        'msTransition' in s ||
                        'OTransition' in s;
                s = null;
                return r;
            })(),

            // WebUploader实例
            uploader;
        if (!WebUploader.Uploader.support('flash') && WebUploader.browser.ie) {

            // flash 安装了但是版本过低。
            if (flashVersion) {
                (function (container) {
                    window['expressinstallcallback'] = function (state) {
                        switch (state) {
                            case 'Download.Cancelled':
                                alert('您取消了更新！')
                                break;

                            case 'Download.Failed':
                                alert('安装失败')
                                break;

                            default:
                                alert('安装已成功，请刷新！');
                                break;
                        }
                        delete window['expressinstallcallback'];
                    };

                    var swf = './expressInstall.swf';
                    // insert flash object
                    var html = '<object type="application/' +
                        'x-shockwave-flash" data="' + swf + '" ';

                    if (WebUploader.browser.ie) {
                        html += 'classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" ';
                    }

                    html += 'width="100%" height="100%" style="outline:0">' +
                        '<param name="movie" value="' + swf + '" />' +
                        '<param name="wmode" value="transparent" />' +
                        '<param name="allowscriptaccess" value="always" />' +
                        '</object>';

                    container.html(html);

                })($wrap);

                // 压根就没有安转。
            } else {
                $wrap.html('<a href="http://www.adobe.com/go/getflashplayer" target="_blank" border="0"><img alt="get flash player" src="http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg" /></a>');
            }

            return;
        } else if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器！');
            return;
        }
      
        // 实例化
        uploader = WebUploader.create({
            pick: {
                id: '#filePicker',
                label: '点击选择文件'
            },
            formData: {
                positionName: positionName
            },
            dnd: '#uploader',
            paste: '#uploader',
            swf: '~/lib/webuploader-0.1.5/Uploader.swf',
            //文件分片上传
            chunked: true,
            chunkRetry: 3,
            prepareNextFile: true,
            chunkSize: '3145728',
            server: '' + ControllerName+'/UpLoad/1111',

            // 禁掉全局的拖拽功能。这样不会出现图片拖进页面的时候，把图片打开。
            disableGlobalDnd: true,
            fileNumLimit: 300,
            fileSizeLimit: 400 * 1024 * 1024,    // 200 M
            fileSingleSizeLimit: 300 * 1024 * 1024,   // 50 M
            // 只允许选择图片文件。
            accept: {
                title: 'Images',
                extensions: 'jpg,png,mp3,wma,rm,wav,mid,ape,flac,rmvb,mp4,flv,wmv,mkv,mov"',
                mimeTypes: 'image/*,video/*'
            }
        });

        // 拖拽时不接受 js, txt 文件。
        uploader.on('dndAccept', function (items) {
            var denied = false,
                len = items.length,
                i = 0,
                // 修改js类型
                unAllowed = 'text/plain;application/javascript ';

            for (; i < len; i++) {
                // 如果在列表里面
                if (~unAllowed.indexOf(items[i].type)) {
                    denied = true;
                    break;
                }
            }
            return !denied;
        });

        uploader.on('dialogOpen', function () {
            console.log('here');
        });

        // 添加“添加文件”的按钮，
        uploader.addButton({
            id: '#filePicker2',
            label: '继续添加'
        });

        uploader.on('ready', function () {
            window.uploader = uploader;
        });

        //编辑文件
        if (id != 0) {
            $.post("" + ControllerName+"/GetFileInfo", { id: id, classifyName: classifyName }, function (res) {
                $placeHolder.addClass('element-invisible');
                $statusBar.show();

                $placeHolder.addClass('element-invisible');
                $('#filePicker2').removeClass('element-invisible');
                $queue.show();
                $statusBar.removeClass('element-invisible');
                uploader.refresh();

                for (var i = 0, l = res.data.length; i < l; i++) {
                    var item = res.data[i];

                    var $item = $('<li class="upload_list"id="' + item.id + '">' +
                        '<img src = "/lib/image-upload/filetype/' + item.suffix.replace('.', '') + '.png" style = "width:40px;height:45px;" />' +
                        '<label>' + item.fileName + '</label> <span class="btns"></span>' +
                        '</li >');

                    $item.find("span.btns").append('<i class="fa fa-check-circle"></i>');
                    $item.find("span.btns").append('<a href="'+ControllerName+'/DownloadFile?fileId=' + item.id + '" ><i class="fa fa-cloud-download" title="下载"  data-value="' + item.id + '" ></i></a><i class="fa fa-minus-circle" title="删除"  data-value="' + item.id + '" ></i>');


                    $item.find('.btns .fa-minus-circle').on('click', function () {
                        var fileId = $(this).attr('data-value');
                        DeleteFile(fileId);
                    });

                    $item.appendTo($queue)
                }
            });
        }

        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {

            var $li = $('<li class="upload_list"id="' + file.id + '">' +
                '<img src = "/lib/image-upload/filetype/' + file.ext + '.png" style = "width:40px;height:45px;" />' +
                '<label>' + file.name + '</label> <span class="btns"><i class="fa fa-minus-circle"></i></span>' +
                '</li >');
            $delbtn = $li.find('i.fa-minus-circle'),
                $prgress = $li.find('p.progress span'),
                $infos = $('<p class="error"></p>'),

                showError = function (code) {
                    switch (code) {
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;

                        case 'interrupt':
                            text = '上传暂停';
                            break;

                        default:
                            text = '上传失败，请重试';
                            break;
                    }

                    $infos.text(text).appendTo($li);
                };

            file.on('statuschange', function (cur, prev) {
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    $li.off('mouseenter mouseleave');
                    //$btns.remove();
                }

                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    console.log(file.statusText);
                    showError(file.statusText);
                    //percentages[ file.id ][ 1 ] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    //$info.remove();
                    $prgress.css('display', 'block');
                    //percentages[ file.id ][ 1 ] = 0;
                } else if (cur === 'progress') {
                    //$info.remove();
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    $prgress.hide().width(0);
                    $li.append('<span class="success"></span>');
                }

                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });

            //$li.on('mouseenter', function () {
            //    $btns.stop().animate({ height: 30 });
            //});

            //$li.on('mouseleave', function () {
            //    $btns.stop().animate({ height: 0 });
            //});

            $delbtn.on('click', function () {
                uploader.removeFile(file);
            });


            $li.appendTo($queue);
        }

        // 负责view的销毁
        function removeFile(file) {
            var $li = $('#' + file.id);
            delete percentages[file.id];
            //updateTotalProgress();
            $li.off().remove();
            updateStatus();
            if (id != 0) {
                $placeHolder.addClass('element-invisible');
                $statusBar.show();
               
                $placeHolder.addClass('element-invisible');
                $('#filePicker2').removeClass('element-invisible');
                $queue.show();
                $statusBar.removeClass('element-invisible');
                uploader.refresh();
            }
        }

        //进度条
        function updateTotalProgress() {
            var loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(percentages, function (k, v) {
                total += v[0];
                loaded += v[0] * v[1];
            });

            percent = total ? loaded / total : 0;


            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        }

        //上传文件统计
        function updateStatus() {
            var text = '', stats;

            if (state === 'ready') {
                text = '选中' + fileCount + '个文件，共' +
                    WebUploader.formatSize(fileSize) + '。';
            } else if (state === 'confirm') {
                stats = uploader.getStats();
                if (stats.uploadFailNum) {
                    text = '已成功上传' + stats.successNum + '个文件，' +
                        stats.uploadFailNum + '个文件上传失败，<a class="retry" href="#">重新上传</a>失败文件或<a class="ignore" href="#">忽略</a>'
                }

            } else {
                stats = uploader.getStats();
                text = '共' + fileCount + '个（' +
                    WebUploader.formatSize(fileSize) +
                    '），已上传' + stats.successNum + '个';

                if (stats.uploadFailNum) {
                    text += '，失败' + stats.uploadFailNum + '个';
                }
            }
            $info.html(text);
        }

        function setState(val) {
            var file, stats;

            if (val === state) {
                return;
            }

            $upload.removeClass('state-' + state);
            $upload.addClass('state-' + val);
            state = val;

            switch (state) {
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    $('#filePicker2').removeClass('element-invisible');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'uploading':
                    $('#filePicker2').addClass('element-invisible');
                    $upload.text('暂停上传');
                    break;

                case 'paused':
                    $upload.text('继续上传');
                    break;

                case 'confirm':
                    $('#filePicker2').removeClass('element-invisible');
                    $upload.text('开始上传');

                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        //alert( '上传成功' );
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }
            updateStatus();
        }

        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id);
            //var $fileItem = $('#lr_form_file_queue_list').find('#lr_filequeue_' + file.id);
            $li.find('.lr-uploader-progress-bar').css('width', (percentage * 100 + '%'));
            //var $li = $('#' + file.id),
            //    $percent = $li.find('.progress span');
            //$percent.css('width', percentage * 100 + '%');
            ////percentages[ file.id ][ 1 ] = percentage;
            //updateTotalProgress();
        };

        //添加文件
        uploader.onFileQueued = function (file) {
            fileCount++;
            fileSize += file.size;

            if (fileCount === 1) {
                $placeHolder.addClass('element-invisible');
                $statusBar.show();
            }

            addFile(file);
            setState('ready');
            //updateTotalProgress();
        };

        //删除文件（未上传）
        uploader.onFileDequeued = function (file) {
            fileCount--;
            fileSize -= file.size;
            if (!fileCount) {
                setState('pedding');
            }
            removeFile(file);
            //updateTotalProgress();

        };

        uploader.on('all', function (type) {
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    break;

                case 'startUpload':
                    setState('uploading');
                    break;

                case 'stopUpload':
                    setState('paused');
                    break;

            }
        });

        //文件上传验证   错误提示 
        uploader.onError = function (code) {
            var msg = '';
            switch (code) {
                case 'Q_EXCEED_SIZE_LIMIT':
                    msg = '文件大小超出限制';
                    break;

                case 'F_EXCEED_SIZE':
                    msg = '文件大小超出';
                    break;

                case 'F_DUPLICATE':
                    msg = '文件已存在';
                    break;

                case 'Q_EXCEED_NUM_LIMIT':
                    msg = '文件总数已达上限';
                    break;
                case 'Q_TYPE_DENIED':
                    msg = '只能上传图片、音频、视频';
                    break

                default:
                    msg = '上传失败，请重试';
                    break;
            }
            //alert( 'Eroor: ' + code );
            alert('Eroor: ' + msg);
        };

        $upload.on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }

            if (state === 'ready') {
                uploader.upload();
            } else if (state === 'paused') {
                uploader.upload();
            } else if (state === 'uploading') {
                uploader.stop();
            }
        });

        $info.on('click', '.retry', function () {
            uploader.retry();
        });

        $info.on('click', '.ignore', function () {
            alert('todo');
        });

        $upload.addClass('state-' + state);
        //updateTotalProgress();

        //上传成功
        uploader.on('uploadSuccess', function (file, response) {
            $.post("" + ControllerName + "/MergeFile", { fileName: file.name, positionName: positionName }, function (res) {
                
                var path = "\\UploadFile\\" + positionName + "\\" + res.path + "\\" + file.name;
                //$('#' + file.id).find('span.btns .fa-minus-circle').remove();
                $('#' + file.id).find('span.btns').append('<i class="fa fa-check-circle"></i>');
                $('#' + file.id).find('span.btns .fa-minus-circle').attr("onclick", 'deleteFileLocal_only(\'' + file.id + '\',\'' + res.path + '\')');
                //$('#' + file.id).find('span.btns').append('<i class="fa fa-minus-circle" title="删除" onclick="deleteFileLocal_only(\'' + file.id + '\',\'' + res.path + '\')"></i>');
                var suffix = "." + file.ext;
              
                //array.push(response.attachment);
                var html = "<input type='text' class='" + file.id + "' name='url' hidden='hidden' value='" + path + "'/>  <input class='" + file.id + "' type='text' name='size' hidden='hidden' value='" + file.size + "'/>  <input type='text' class='" + file.id + "' name='suffix' hidden='hidden' value='" + suffix + "'/> <input type='text' class='" + file.id +"' name='names' hidden='hidden' value='" + file.name + "'/>";
                $("#filelist").append(html);
                
            })
        });
       


        //开始上传时
        uploader.on('uploadStart', function (file) {
            var $li = $('#' + file.id);
            $li.append('<div class="lr-uploader-progress"><div class="lr-uploader-progress-bar" style="width:0%;"></div></div>')
        });
        //删除文件（已上传文件）
        function DeleteFile(fileID) {
            $.post("" + ControllerName+"/DeleteFile", { fileId: fileID }, function (res) {
                if (res == "ok") {
                    var $li = $('#' + fileID);
                    $li.off().remove();
                } else {
                    alert("表单未提交保存");
                }
            })
        }
    });
})(jQuery);
