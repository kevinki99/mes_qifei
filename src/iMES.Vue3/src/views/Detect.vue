<template>
    <div class="mode-changer">
        <el-text class="mx-1" tag="b">图片导入模式：</el-text>
        <el-switch v-model="isAuto" class="mb-2" style="--el-switch-on-color: #13ce66; --el-switch-off-color: #ff4949"
            active-text="自动" inactive-text="手动" />
    </div>
    <div class="image-uploader">
        <el-button type="primary" @click="triggerFileInput" v-if="isAuto === false">上传图片</el-button>
        <input type="file" ref="fileInput" @change="onFileChange" accept="image/*" v-show="false">
        <div class="image-container">
            <img v-if="imageUrl" :src="imageUrl" alt="Uploaded Image" class="uploaded-image" />
            <div v-else class="placeholder">请上传图片</div>
        </div>

        <div>
            <el-text class="mx-1"> 检测结果：</el-text>
            <span v-if="detectionResultType === 'success'">
                <el-icon>
                    <CircleCheck color="green" />
                </el-icon>
                <el-text class="mx-1" type="success"> {{ detectionResultDetail }}</el-text>
            </span>
            <span v-else-if="detectionResultType === 'fail'">
                <el-icon>
                    <CircleClose color="red" />
                </el-icon>
                <el-text class="mx-1" type="danger"> {{ detectionResultDetail }}</el-text>
            </span>
            <span v-else>
                <el-icon>
                    <Warning color="#FFD700" />
                </el-icon>
                <el-text class="mx-1" type="warning"> {{ detectionResultDetail }}</el-text>
            </span>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            isAuto: false,
            imageUrl: null,
            detectionResultDetail: '无数据',
            detectionResultType: 'warning'
        };
    },
    methods: {
        triggerFileInput() {
            this.$refs.fileInput.click();
        },
        async onFileChange(e) {
            const file = e.target.files[0];
            if (!file) {
                return;
            }
            this.imageUrl = URL.createObjectURL(file);

            const API_KEY = "wPulweI6szJ9pKLOC2FNobxw";
            const SECRET_KEY = "OknefjA9E5Fm1lQKdUj1zfhXhE1t0gkx";
            const MODEL_API_URL = "https://aip.baidubce.com/rpc/2.0/ai_custom/v1/classification/wire_classification";
            let ACCESS_TOKEN = "24.79547a46130237e3fda87d43096c6598.2592000.1719126931.282335-74268983";

            const base64Image = await this.convertToBase64(file);
            const PARAMS = { top_num: 1, image: base64Image };

            if (!ACCESS_TOKEN) {
                ACCESS_TOKEN = await this.getAccessToken(API_KEY, SECRET_KEY);
            }

            const result = await this.classifyImage(ACCESS_TOKEN, MODEL_API_URL, PARAMS);
            const name = result.results[0].name;
            if (name === "合格") {
                this.detectionResultDetail = "正确";
                this.detectionResultType = "success";
            } else {
                this.detectionResultDetail = `错误 错误类型: ${name}`;
                this.detectionResultType = "fail";
            }
        },
        async convertToBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.onload = (e) => resolve(e.target.result.split(',')[1]);
                reader.onerror = reject;
                reader.readAsDataURL(file);
            });
        },
        async getAccessToken(apiKey, secretKey) {
            const authUrl = `https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id=${apiKey}&client_secret=${secretKey}`;
            try {
                const response = await fetch(authUrl);
                const data = await response.json();
                return data.access_token;
            } catch (error) {
                console.error("Failed to get access token:", error);
                throw error;
            }
        },
        async classifyImage(accessToken, apiUrl, params) {
            const requestUrl = `${apiUrl}?access_token=${accessToken}`;
            try {
                const response = await fetch(requestUrl, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(params)
                });
                const data = await response.json();
                return data;
            } catch (error) {
                console.error("Failed to classify image:", error);
                throw error;
            }
        }
    }
};
</script>

<style scoped>
.mode-changer {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    gap: 20px;
}

.image-uploader {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 20px;
}

.image-container {
    width: 600px;
    height: 600px;
    overflow: hidden;
    display: flex;
    justify-content: center;
    align-items: center;
    border: 2px dashed #ccc;
    position: relative;
}

.uploaded-image {
    max-width: 100%;
    max-height: 100%;
}

.placeholder {
    text-align: center;
    color: #aaa;
}

.el-icon {
    vertical-align: middle;
    font-size: 20px;
}

.el-text {
    vertical-align: middle;
    font-size: 20px;
}
</style>