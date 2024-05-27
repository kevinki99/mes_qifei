<template>
  <div class="flow-demo">
    <el-divider>发起流程</el-divider>

    <mes-form
      style="margin-top:20px;"
      ref="form"
      :load-key="true"
      :label-width="60"
      :formRules="options"
      :formFields="fields"
    ></mes-form>
    <div style="text-align:center">
      <el-button @click="submit" type="primary">提交</el-button>
    </div>
  </div>
</template>
<script>
import {
  defineComponent,
  ref,
  reactive,
  toRefs,
  getCurrentInstance
} from 'vue';
import { useRouter, useRoute } from 'vue-router';
import http from '@/../src/api/http.js';
import MesForm from '@/components/basic/MesForm.vue';
export default {
  components: {
    MesForm
  },
  setup() {
    const fields = reactive({
      ExpertName: '',
      Enable: '',
      City: '',
      IDNumber: '',
      PhoneNo: '',
      CreateDate: '',
      Resume: '',
      HeadImageUrl: ''
    });
    const options = reactive([
      [{ title: '名称', field: 'ExpertName' }],
      [
        {
          dataKey: 'enable',
          data: [],
          title: '状态',
          required: true,
          field: 'Enable',
          type: 'select'
        }
      ],
      [
        {
          dataKey: 'city',
          data: [],
          title: '地区',
          field: 'City',
          type: 'select'
        }
      ],
      [{ title: '日期', field: 'CreateDate', type:"date" }],
      [{ title: '备注', field: 'Resume', colSize: 12, type: 'textarea' }]
    ]);
    let appContext = getCurrentInstance().appContext;
    let $message = appContext.config.globalProperties.$message;
    const form = ref(null);

    const submit = () => {
      form.value.validate(() => {
        http
          .post('api/App_Expert/add', { mainData: fields }, true)
          .then((result) => {
            if (!result.status) {
              return $message.error(result.message);
            }
            form.value.reset();
            $message.success(result.message);
          });
      });
    };

    return {
      options,
      fields,
      submit,
      form
    };
  }
};
</script>
<style scoped lang="less">
.flow-demo {
  position: absolute;
  width: 600px;
  left: 0;
  right: 0;
  margin: 0 auto;
  margin-top: 20px;
}
</style>
