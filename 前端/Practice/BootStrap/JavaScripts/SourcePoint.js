
(function ($) {
    $.SourcePoint = {};

    //�Խڵ��е������滻����
    function Replace(config) {

        var replaceConfig = {
            SourceItems: [],
            //�滻����Ĭ��Ϊ�����滻
            MacthRule: MacthRule = /[\u4e00-\u9fa5]+/g
        };
        UseConfiguration(config);

        //�洢��Ҫ�����滻������
        this.SourceItems = replaceConfig.SourceItems;
        //�滻����Ĭ��Ϊ�����滻
        this.MacthRule = replaceConfig.MacthRule;
        /**�Խڵ�� html�����滻
          * @param {Node} nodes �ڵ�
          * @param {bool} match true:��������ƥ��
          * @returns {void}
          */
        this.HtmlReplace = function (nodes, match) {
            $(nodes).each(function () {
                $(this).html(ReplaceContext($(this).html(), match));
            });
        };
        /** �Խڵ�� text �����滻
          * @param {Node} nodes �ڵ�
          * @param {bool} match true:��������ƥ��
          */
        this.TextReplace = function (nodes, match) {
            $(nodes).each(function () {
                $(this).text(ReplaceContext($(this).text(), match));
            });
        };
        /** �Խڵ�� ���Խ����滻
         * @param {Node} nodes �ڵ�
         * @param {bool} match true:��������ƥ��
         */
        this.AttributeReplace = function (str, match) {
            $("[" + str + "]").each(function () {
                $(this).attr(str, ReplaceContext($(this).attr(str), match));

            });
        };

        /** �Խڵ��е�ֵ�����滻
         * @param {Node} nodes �ڵ�
         * @param {bool} match true:��������ƥ��
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
         * @description �滻����
         * @param {string} strHtml ����
         * @param {bool} match true:ʹ������ƥ��
         * @returns {string} strHtml �滻��Ľ��
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
         * @description ʹ������
         * @param {Object} config ������Ϣ
         */
        function UseConfiguration(config) {
            $.extend(replaceConfig, config);
            CreateItemKeys();
        }

        /*
         * @description ���� key
         */
        function CreateItemKeys() {
            replaceConfig.SourceKeys = replaceConfig.SourceItems.map(function (v) { return v.Key; });
        }

        /**
         * @description ����滻�� key
         * @param {Array} objs 
         */
        function AddItems(objs) {
            replaceConfig.SourceItems = replaceConfig.SourceItems.concat(objs);
            CreateItemKeys();
        }
        return this;
    };

    /*
     * @description �Խڵ���� ����
     * @param {MutationObserverInit} ������Ҫ�����Ķ���
     */
    function Monitor(options) {

        var configuation = {
            /*
             * �����Ҫ�۲�Ŀ��ڵ���ӽڵ�(������ĳ���ӽڵ�,�����Ƴ���ĳ���ӽڵ�),������Ϊtrue.
             */
            childList: true,
            /*
             *�����Ҫ�۲�Ŀ��ڵ�����Խڵ�(������ɾ����ĳ������,�Լ�ĳ�����Ե�����ֵ�����˱仯),������Ϊtrue.
             */
            attributes: true,
            /*
             * ���Ŀ��ڵ�ΪcharacterData�ڵ�(һ�ֳ���ӿ�,�������Ϊ�ı��ڵ�,ע�ͽڵ�,�Լ�����ָ��ڵ�)ʱ,
             * ҲҪ�۲�ýڵ���ı������Ƿ����仯,
             * ������Ϊtrue.
             */
            characterData: true,
            /*
             * ����Ŀ��ڵ�,
             * �������Ҫ�۲�Ŀ��ڵ�����к���ڵ�(�۲�Ŀ��ڵ�������������DOM���ϵ��������ֽڵ�仯),
             * ������Ϊtrue.
             */
            subtree: true,
            /*
             * ��attributes�����Ѿ���Ϊtrue��ǰ����,
             * �����Ҫ�������仯�����Խڵ�֮ǰ������ֵ��¼����(��¼������MutationRecord�����oldValue������),
             * ������Ϊtrue.
             */
            attributeOldValue: true,
            /*
             * ��characterData�����Ѿ���Ϊtrue��ǰ����,
             * �����Ҫ�������仯��characterData�ڵ�֮ǰ���ı����ݼ�¼����(��¼������MutationRecord�����oldValue������),
             * ������Ϊtrue.
             */
            characterDataOldValue: true,
            /*
             * һ������������(����Ҫָ�������ռ�),
             * ֻ�и������а����������������仯ʱ�Żᱻ�۲쵽,
             * �������Ƶ����Է����仯��ᱻ����.
             */
            attributeFilter:[]
        };

        configuation = options ? $.extend({}, options) : configuation;
        console.debug(configuation)
        /** 
         * @description �����ڵ�仯
         * @param {Node} node �ڵ�
         * @param {function(records,own)} callback �������仯 ��Ļص�
         * @param {MutationObserverInit} options
         * records�����������ɸ�MutationRecord���������
         * own������۲��߶�����
         * @returns {MutationObserver} observice �۲��߶���
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
     * @property {Object}  Monitoring �� dom �ڵ���� ���� ֻ��ʹ�� dom����
     */
    $.SourcePoint.Monitor = Monitor;

}(jQuery))