
(function ($) {
    $.SourcePoint = {};

    //对节点中的内容替换辅助
    function Replace(config) {

        var replaceConfig = {
            SourceItems: [],
            //替换规则，默认为中文替换
            MacthRule: MacthRule = /[\u4e00-\u9fa5]+/g
        };
        UseConfiguration(config);

        //存储需要进行替换的数据
        this.SourceItems = replaceConfig.SourceItems;
        //替换规则，默认为中文替换
        this.MacthRule = replaceConfig.MacthRule;
        /**对节点的 html进行替换
          * @param {Node} nodes 节点
          * @param {bool} match true:采用正则匹配
          * @returns {void}
          */
        this.HtmlReplace = function (nodes, match) {
            $(nodes).each(function () {
                $(this).html(ReplaceContext($(this).html(), match));
            });
        };
        /** 对节点的 text 进行替换
          * @param {Node} nodes 节点
          * @param {bool} match true:采用正则匹配
          */
        this.TextReplace = function (nodes, match) {
            $(nodes).each(function () {
                $(this).text(ReplaceContext($(this).text(), match));
            });
        };
        /** 对节点的 属性进行替换
         * @param {Node} nodes 节点
         * @param {bool} match true:采用正则匹配
         */
        this.AttributeReplace = function (str, match) {
            $("[" + str + "]").each(function () {
                $(this).attr(str, ReplaceContext($(this).attr(str), match));

            });
        };

        /** 对节点中的值进行替换
         * @param {Node} nodes 节点
         * @param {bool} match true:采用正则匹配
         */
        this.ValueReplace = function (nodes, match) {
            $(nodes).each(function () {
                $(this).val(ReplaceContext($(this).val(), match));
            });
        };
        this.ReplaceContext = ReplaceContext;
        this.UpdateConfig = UseConfiguration;
        this.UpdateItems = AddItems;

        /**
         * @description 替换内容
         * @param {string} strHtml 内容
         * @param {bool} match true:使用正则匹配
         * @returns {string} strHtml 替换后的结果
         */
        function ReplaceContext(strHtml, match) {
            strHtml = strHtml.trim();
            var items = [strHtml];
            if (match) items = strHtml.match(replaceConfig.MacthRule);
            var keyIndex = $.inArray(strHtml, replaceConfig.SourceKeys);

            var isStartReplace = match || keyIndex > -1;

            if (isStartReplace == false) return strHtml;
            var isReplaceOk = false;

            replaceConfig.SourceItems.forEach(function (item, index) {
                if (isReplaceOk == false || match) {
                    var isReplace = match || keyIndex == index;
                    if (match) isReplace = $.inArray(item.Key, items) > -1;
                    if (isReplace) {
                        strHtml = strHtml.replace(item.Key, item.Value);
                        isReplaceOk = true;
                    }
                }
            });
            return strHtml;
        };

        /**
         * @description 使用配置
         * @param {Object} config 配置信息
         */
        function UseConfiguration(config) {
            $.extend(replaceConfig, config);
            CreateItemKeys();
        }

        /*
         * @description 生成 key
         */
        function CreateItemKeys() {
            replaceConfig.SourceKeys = replaceConfig.SourceItems.map(function (v) { return v.Key; });
        }

        /**
         * @description 添加替换的 key
         * @param {Array} objs 
         */
        function AddItems(objs) {
            replaceConfig.SourceItems = replaceConfig.SourceItems.concat(objs);
            CreateItemKeys();
        }
        return this;
    };

    /*
     * @description 对节点进行 监听
     * @param {MutationObserverInit} 设置需要监听的对象
     */
    function Monitor(options) {

        var configuation = {
            /*
             * 如果需要观察目标节点的子节点(新增了某个子节点,或者移除了某个子节点),则设置为true.
             */
            childList: true,
            /*
             *如果需要观察目标节点的属性节点(新增或删除了某个属性,以及某个属性的属性值发生了变化),则设置为true.
             */
            attributes: true,
            /*
             * 如果目标节点为characterData节点(一种抽象接口,具体可以为文本节点,注释节点,以及处理指令节点)时,
             * 也要观察该节点的文本内容是否发生变化,
             * 则设置为true.
             */
            characterData: true,
            /*
             * 除了目标节点,
             * 如果还需要观察目标节点的所有后代节点(观察目标节点所包含的整棵DOM树上的上述三种节点变化),
             * 则设置为true.
             */
            subtree: true,
            /*
             * 在attributes属性已经设为true的前提下,
             * 如果需要将发生变化的属性节点之前的属性值记录下来(记录到下面MutationRecord对象的oldValue属性中),
             * 则设置为true.
             */
            attributeOldValue: true,
            /*
             * 在characterData属性已经设为true的前提下,
             * 如果需要将发生变化的characterData节点之前的文本内容记录下来(记录到下面MutationRecord对象的oldValue属性中),
             * 则设置为true.
             */
            characterDataOldValue: true,
            /*
             * 一个属性名数组(不需要指定命名空间),
             * 只有该数组中包含的属性名发生变化时才会被观察到,
             * 其他名称的属性发生变化后会被忽略.
             */
            attributeFilter:[]
        };

        configuation = options ? $.extend({}, options) : configuation;
        console.debug(configuation)
        /** 
         * @description 监听节点变化
         * @param {Node} node 节点
         * @param {function(records,own)} callback 监听到变化 后的回调
         * @param {MutationObserverInit} options
         * records：包含了若干个MutationRecord对象的数组
         * own：这个观察者对象本身
         * @returns {MutationObserver} observice 观察者对象
         */
        this.Observe = function (node, callback, options) {
            if (typeof callback != "function") {
                throw new Error("The second parameter must be the function")
                return;
            }

            var ops = options ? options : configuation;

            var observice = new MutationObserver(callback);
            console.debug(ops)
            observice.observe(node, ops);
            return observice;
        };

        return this;
    };


    $.SourcePoint.Replace = Replace;
    /**
     * @property {Object}  Monitoring 对 dom 节点进行 监听 只能使用 dom类型
     */
    $.SourcePoint.Monitor = Monitor;

}(jQuery))