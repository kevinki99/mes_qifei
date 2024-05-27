<!--
*Author：COCO
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *业务请在@/extension/custom/custom/Base_MaterialDetailTree.js此处编写
 -->
<template>
    <view-grid ref="grid"
               :columns="columns"
               :detail="detail"
               :editFormFields="editFormFields"
               :editFormOptions="editFormOptions"
               :searchFormFields="searchFormFields"
               :searchFormOptions="searchFormOptions"
               :table="table"
               :extend="extend">
    </view-grid>
</template>
<script>
    import extend from "@/extension/custom/custom/Base_MaterialDetailTree.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'MaterialDetailTree_Id',
                footer: "Foots",
                cnName: '物料清单(Tree)',
                name: 'custom/Base_MaterialDetailTree',
                url: "/Base_MaterialDetailTree/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"ProductName":"","ProductCode":"","ProductStandard":"","Unit_Id":"","QuantityPer":"","ParentId":[]});
            const editFormOptions = ref([[{"title":"产品名称","required":true,"field":"ProductName","type":"text"}],
                              [{"title":"产品编码","required":true,"field":"ProductCode","type":"text"}],
                              [{"title":"产品规格","field":"ProductStandard","type":"text"}],
                              [{"dataKey":"unitList","data":[],"title":"单位","field":"Unit_Id","type":"select"}],
                              [{"title":"单位用量","required":true,"field":"QuantityPer","type":"number"}],
                              [{"dataKey":"materialDetail","data":[],"title":"父分类","field":"ParentId","type":"cascader"}]]);
            const searchFormFields = ref({"ProductName":"","ProductCode":"","ProductStandard":""});
            const searchFormOptions = ref([[{"title":"产品名称","field":"ProductName","type":"like"},{"title":"产品编码","field":"ProductCode","type":"like"},{"title":"产品规格","field":"ProductStandard","type":"like"}]]);
            const columns = ref([{field:'MaterialDetailTree_Id',title:'物料清单主键ID',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'ProductName',title:'产品名称',type:'string',sort:true,width:120,require:true,align:'left',sort:true},
                       {field:'ProductCode',title:'产品编码',type:'string',link:true,sort:true,width:120,require:true,align:'left'},
                       {field:'ProductStandard',title:'产品规格',type:'string',sort:true,width:120,align:'left'},
                       {field:'Unit_Id',title:'单位',type:'int',bind:{ key:'unitList',data:[]},width:110,align:'left'},
                       {field:'QuantityPer',title:'单位用量',type:'int',width:110,require:true,align:'left'},
                       {field:'ParentId',title:'父分类',type:'guid',bind:{ key:'materialDetail',data:[]},width:110,align:'left'},
                       {field:'CreateID',title:'创建人编号',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'创建人',type:'string',width:130,align:'left'},
                       {field:'CreateDate',title:'创建时间',type:'datetime',width:110,align:'left',sort:true},
                       {field:'ModifyID',title:'修改人编号',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:130,align:'left'},
                       {field:'ModifyDate',title:'修改时间',type:'datetime',width:110,align:'left',sort:true}]);
            const detail = ref({
                cnName: "#detailCnName",
                table: "#detailTable",
                columns: [],
                sortName: "",
                key: ""
            });
            return {
                table,
                extend,
                editFormFields,
                editFormOptions,
                searchFormFields,
                searchFormOptions,
                columns,
                detail,
            };
        },
    });
</script>
