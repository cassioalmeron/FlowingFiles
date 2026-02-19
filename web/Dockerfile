# Multi-stage build for React/Vite frontend
FROM node:20-alpine AS build

WORKDIR /app
COPY package*.json ./
RUN npm ci

COPY . .

# Accept build arguments for Vite environment variables
# Placed AFTER COPY . . to optimize layer caching
# Changing these values won't invalidate npm ci cache
ARG VITE_API_URL=http://localhost:5000
ARG VITE_APP_NAME=FlowingFiles
ENV VITE_API_URL=${VITE_API_URL}
ENV VITE_APP_NAME=${VITE_APP_NAME}

RUN npm run build

# Production runtime with Nginx
FROM nginx:alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
