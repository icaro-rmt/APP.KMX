// Home page functionality
class FileUploadManager {
    constructor() {
        this.fileInput = document.getElementById('fileInput');
        this.uploadButton = document.querySelector('.button-selecionar');
        this.uploadStatus = document.getElementById('uploadStatus');
        this.maxFileSize = 10 * 1024 * 1024; // 10MB
        this.allowedExtensions = ['.xls', '.xlsx', '.kml', '.kmz'];
        
        this.init();
    }

    init() {
        if (this.fileInput) {
            this.fileInput.addEventListener('change', (e) => this.handleFileSelection(e));
        }
    }

    handleFileSelection(event) {
        const file = event.target.files[0];
        if (file) {
            this.uploadFile(file);
        }
    }

    validateFile(file) {
        const fileExtension = '.' + file.name.split('.').pop().toLowerCase();
        
        if (!this.allowedExtensions.includes(fileExtension)) {
            return { isValid: false, message: 'Formato de arquivo não suportado. Use: .xls, .xlsx, .kml ou .kmz' };
        }

        if (file.size > this.maxFileSize) {
            return { isValid: false, message: 'Arquivo muito grande. Tamanho máximo: 10MB' };
        }

        return { isValid: true };
    }

    async uploadFile(file) {
        const validation = this.validateFile(file);
        if (!validation.isValid) {
            this.showStatus(validation.message, 'error');
            return;
        }

        this.showStatus('Enviando arquivo...', 'loading');
        this.setButtonState(true);

        try {
            const formData = new FormData();
            formData.append('file', file);

            const response = await fetch('/File/Upload', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                const blob = await response.blob();
                this.downloadFile(blob, file.name);
                this.showStatus('Arquivo convertido e baixado com sucesso!', 'success');
            } else {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || 'Erro durante o upload do arquivo.');
            }
        } catch (error) {
            console.error('Upload error:', error);
            this.showStatus('Erro ao converter arquivo: ' + error.message, 'error');
        } finally {
            this.setButtonState(false);
            this.fileInput.value = '';
        }
    }

    downloadFile(blob, originalFileName) {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        
        const originalName = originalFileName.substring(0, originalFileName.lastIndexOf('.'));
        const inputExtension = '.' + originalFileName.split('.').pop().toLowerCase();
        const convertedExtension = this.getConvertedExtension(inputExtension);
        a.download = `${originalName}-converted${convertedExtension}`;
        
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
    }

    getConvertedExtension(inputExtension) {
        if (inputExtension === '.xls' || inputExtension === '.xlsx') {
            return '.kml';
        }
        if (inputExtension === '.kml' || inputExtension === '.kmz') {
            return '.xls';
        }
        return '.converted';
    }

    showStatus(message, type) {
        this.uploadStatus.textContent = message;
        this.uploadStatus.className = `upload-status ${type}`;
        
        if (type === 'success' || type === 'error') {
            setTimeout(() => {
                this.uploadStatus.textContent = '';
                this.uploadStatus.className = 'upload-status';
            }, 5000);
        }
    }

    setButtonState(disabled) {
        this.uploadButton.disabled = disabled;
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    new FileUploadManager();
});
