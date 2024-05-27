<template>
    <iframe marginwidth="0" :src="url" class="tabiframe" marginheight="0" frameborder="0" scrolling="no" onload="changeFrameHeight()" height="100%" id="main" name="main" width="100%" style="min-height: 600px"></iframe>
</template>

<script>
    module.exports = {
        props: ['pdata'],
        data: function () {
            return {
               url:"",
            }
        },
        methods: {
         
        },
        mounted: function () {
            var pro = this;
            pro.$nextTick(function () {
                pro.url = pro.pdata.PageUrl;
             
            })

        },
        watch: {
            "pdata.PageUrl": { //深度监听，可监听到对象、数组的变化
                handler(newV, oldV) {
                    if (newV != oldV) {
                        this.url = newV;
                    }
                },
                deep: true
            }
        }
      
    };
</script>