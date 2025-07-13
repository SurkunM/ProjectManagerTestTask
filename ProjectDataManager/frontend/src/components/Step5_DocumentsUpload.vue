<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 5: Project Documents</h3>
            </div>
            <div class="card-body">
                <div>
                    <div class="upload-area card"
                         @dragover.prevent="dragover"
                         @dragleave.prevent="dragleave"
                         @drop.prevent="drop"
                         :class="{ 'border-primary': isDragover, 'bg-light': isDragover }">
                        <div class="card-body text-center p-5">
                            <input type="file"
                                   multiple
                                   @change="handleFileSelect"
                                   ref="fileInput"
                                   class="d-none" />
                            <div class="mb-3">
                                <i class="bi bi-cloud-arrow-up display-4 text-muted"></i>
                            </div>
                            <p class="lead mb-3">Drag and drop the files here or click to select</p>

                            <button @click="$refs.fileInput.click()" class="btn btn-primary px-4">
                                <i class="bi bi-folder2-open me-2"></i>Select Files
                            </button>

                            <div v-if="files.length" class="mt-4">
                                <h5 class="text-start mb-3">Selected files:</h5>
                                <ul class="list-group">
                                    <li v-for="(file, index) in files" :key="index"
                                        class="list-group-item d-flex justify-content-between align-items-center">
                                        <div>
                                            <i class="bi bi-file-earmark me-2"></i>
                                            {{ file.name }}
                                            <small class="text-muted ms-2">({{ formatFileSize(file.size) }})</small>
                                        </div>
                                        <button @click="removeFile(index)" class="btn btn-sm btn-outline-danger">
                                            <i class="bi bi-x-lg"></i>
                                        </button>
                                    </li>
                                </ul>

                                <div class="d-grid mt-3">
                                    <button class="btn btn-success" @click="uploadFiles">
                                        <i class="bi bi-upload me-2"></i>Upload files
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="d-flex justify-content-between  mt-4">
                        <button class="btn btn-secondary px-4" @click="prevStep">
                            Back
                        </button>
                        <button class="btn btn-primary px-4" @click="nextStep">
                            Finish
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from "axios";

    export default {

        data() {
            return {
                isDragover: false,
                files: []
            };
        },
        methods: {
            dragover() {
                this.isDragover = true;
            },
            dragleave() {
                this.isDragover = false;
            },
            drop(e) {
                this.isDragover = false;
                this.addFiles(e.dataTransfer.files);
            },
            handleFileSelect(e) {
                this.addFiles(e.target.files);
            },
            addFiles(files) {
                this.files = [...this.files, ...Array.from(files)];
            },
            removeFile(index) {
                this.files.splice(index, 1);
            },

            prevStep() {
                this.$router.push("/executers");
            },

            async uploadFiles() {
                const formData = new FormData();
                this.files.forEach(file => {
                    formData.append('files[]', file);
                });

                try {
                    await axios.post('/api/upload', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    });
                    alert('Файлы загружены!');
                    this.files = [];
                } catch (error) {
                    console.error('Ошибка загрузки:', error);
                }
            }
        }
    };
</script>

<style>
    .upload-area {
        border: 2px dashed #ccc;
        padding: 20px;
        text-align: center;
    }

        .upload-area.is-dragover {
            border-color: #42b983;
            background: #f0f8ff;
        }
</style>