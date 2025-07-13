<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 5: Project Documents</h3>
            </div>
            <div class="card-body">
                <div class="upload-area p-5 text-center"
                     @dragover.prevent="onDragOver"
                     @dragleave.prevent="onDragLeave"
                     @drop.prevent="onDrop">
                    <input type="file"
                           ref="fileInput"
                           @change="handleFileSelect"
                           class="d-none"/>

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
                    <button class="btn btn-primary px-4" @click="nextStep">
                        Finish
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
                selectedFile: null,
                isDragging: false
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

            onDragOver() {
                this.isDragging = true;
            },

            onDragLeave() {
                this.isDragging = false;
            },

            onDrop(event) {
                this.isDragging = false;
                const droppedFile = event.dataTransfer.files[0];
                if (droppedFile) {
                    this.selectedFile = droppedFile;
                }
            },

            formatFileSize(sizeInBytes) {
                const units = ["байт", "КБ", "МБ"];
                let unitIndex = 0;
                while (sizeInBytes >= 1024 && unitIndex < units.length - 1) {
                    sizeInBytes /= 1024;
                    unitIndex++;
                }
                return `${Math.round(sizeInBytes * 100) / 100} ${units[unitIndex]}`;
            },

            prevStep() {
                this.$router.push("/executers");
            },

            async uploadFile() {
                if (!this.selectedFile) {
                    alert('Файл не выбран');
                    return;
                }

                const formData = new FormData();
                formData.append('projectDocument', this.selectedFile);

                try {
                    const response = await fetch('/api/upload', {
                        method: 'POST',
                        body: formData
                    });

                    if (response.ok) {
                        alert('Документ успешно загружен!');
                    } else {
                        alert('Ошибка при загрузке файла');
                    }
                } catch (err) {
                    alert('Что-то пошло не так: ' + err.message);
                }
            }
        }
    };
</script>