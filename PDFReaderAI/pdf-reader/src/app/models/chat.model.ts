export interface Chat {
    id?: string;
    title: string;
    prompts: string[];
    responses: string[];
    timeOfDiscussion: Date | string;
    fileName?: string | null;
    fileContent?: Uint8Array | null;
    fileMimeType?: string | null;
    fileBase64Content?: string;
}