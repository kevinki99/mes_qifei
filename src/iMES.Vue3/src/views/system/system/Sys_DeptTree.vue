<!--
*Author：COCO
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *业务请在@/extension/system/system/Sys_DeptTree.js此处编写
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
    import extend from "@/extension/system/system/Sys_DeptTree.js";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'DeptTreeId',
                footer: "Foots",
                cnName: '部门管理(Tree)',
                name: 'system/Sys_DeptTree',
                url: "/Sys_DeptTree/",
                sortName: "CreateDate"
            });
            const editFormFields = ref({"DeptName":"","DeptCode":"","ParentId":[]});
            const editFormOptions = ref([[{"title":"部门名称","required":true,"field":"DeptName","type":"text"}],
                              [{"title":"部门编码","required":true,"field":"DeptCode","type":"text"}],
                              [{"dataKey":"deptLevel","data":[],"title":"父分类","field":"ParentId","type":"cascader"}]]);
            const searchFormFields = ref({"DeptName":"","DeptCode":""});
            const searchFormOptions = ref([[{"title":"部门名称","field":"DeptName","type":"like"},{"title":"部门编码","field":"DeptCode","type":"like"}]]);
            const columns = ref([{field:'DeptTreeId',title:'部门主键ID',type:'guid',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'DeptName',title:'部门名称',type:'string',link:true,width:120,require:true,align:'left',sort:true},
                       {field:'DeptCode',title:'部门编码',type:'string',width:120,require:true,align:'left'},
                       {field:'ParentId',title:'父分类',type:'guid',bind:{ key:'deptLevel',data:[]},width:110,align:'left'},
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
