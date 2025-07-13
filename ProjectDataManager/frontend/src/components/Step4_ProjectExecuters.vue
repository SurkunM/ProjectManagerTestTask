<template>
    <div class="container mt-5">
        <div class="card">
            <div class="card-header">
                <h3>Step 4: Team Members</h3>
            </div>
            <div class="card-body">
                <div>
                    <div class="mb-3">
                        <label for="projectName" class="form-label fs-5">Employee</label>
                        <input type="text"
                               class="form-control"
                               id="projectName"
                               v-model="project.name"
                               autocomplete="off">
                    </div>

                    <div class="d-flex justify-content-between  mt-4">
                        <button class="btn btn-secondary px-4" @click="prevStep">
                            Back
                        </button>
                        <button class="btn btn-primary px-4" @click="nextStep">
                            Next
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>export default {
        data() {
            return {
                project: {
                    name: '',
                    name2: "",
                    startDate: '',
                    endDate: '',
                    priority: '2'
                },
                steps: [
                    { title: 'Основная информация', completed: false },
                    // Добавьте другие шаги здесь
                    { title: 'Завершение', completed: false }
                ]
            }
        },

        methods: {
            nextStep() {
                if (!this.project.name.trim()) {
                    alert('Пожалуйста, введите название проекта');
                    return;
                }

                this.$router.push("/documents");
            },

            prevStep() {
                this.$router.push("/manager");
            },
        }
    }</script>
