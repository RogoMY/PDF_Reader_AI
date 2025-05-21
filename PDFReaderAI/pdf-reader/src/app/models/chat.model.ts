// chat.model.ts
export interface Chat {
  id?: string;
  title: string;
  prompts: string[];
  responses: string[];
  timeOfDiscussion: Date | string;
  fileName?: string | null;
  fileContent?: string | null;     // ← string Base64
  fileMimeType?: string | null;
}
