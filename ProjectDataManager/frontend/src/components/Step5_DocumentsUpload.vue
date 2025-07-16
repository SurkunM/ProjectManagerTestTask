<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 5: Project Documents</h3>
            </div>
            <div class="card-body">
                <div class="upload-area p-5 text-center"
                     @dragover.prevent=""
                     @dragleave.prevent=""
                     @drop.prevent="onDrop">
                    <input type="file"
                           ref="fileInput"
                           @change="handleFileSelect"
                           class="d-none" />

                    <div v-if="!selectedFile">
                        <p class="lead mb-3">Drag a file here or choose manually.</p>
                        <button @click="openFileDialog" class="btn btn-primary px-4">
                            <i class="bi bi-folder2-open me-2"></i>Choose File
                        </button>
                    </div>

                    <div v-else class="mt-4">
                        <h5>Selected File:</h5>
                        <p>{{ selectedFile.name }} ({{ formatFileSize(selectedFile.size) }})</p>
                        <button @click="clearSelection" class="btn btn-link">Clear Selection</button>
                    </div>
                </div>

                <div class="mt-4">
                    <button @click="uploadFile" class="btn btn-success w-100" :disabled="!selectedFile">
                        <i class="bi bi-upload me-2"></i>Upload File
                    </button>
                </div>

                <div class="d-flex justify-content-between  mt-4">
                    <button class="btn btn-secondary px-4" @click="prevStep">
                        Back
                    </button>
                    <button class="btn btn-primary px-4" @click="createProject">
                        Create
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {

        data() {
            return {
                selectedFile: null
            };
        },
        methods: {
            openFileDialog() {
                this.$refs.fileInput.click();
            },

            clearSelection() {
                this.selectedFile = null;
            },

            handleFileSelect() {
                this.selectedFile = event.target.files[0];
            },

            onDrop(event) {
                const droppedFile = event.dataTransfer.files[0];

                if (droppedFile) {
                    this.selectedFile = droppedFile;
                }
            },

            formatFileSize(sizeInBytes) {
                const units = ["bytes", "KB", "MB"];
                let index = 0;

                while (sizeInBytes >= 1024 && index < units.length - 1) {
                    sizeInBytes /= 1024;
                    index++;
                }

                return `${Math.round(sizeInBytes * 100) / 100} ${units[index]}`;
            },

            prevStep() {
                this.$router.push("/executers");
            },

            createProject(){
                this.$store.dispatch("createProject")
                .then(() => {
                    alert("Success");

                    this.$store.commit("resetFields")
                    this.$router.push("/");                    
                });
            },

            uploadFile() {
                alert("File is uploaded");
            }
        }
    };
</script>