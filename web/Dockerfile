# Stage 1: Build the React application
FROM node:20-alpine AS build

WORKDIR /app

# Copy package files first for better caching
COPY package.json package-lock.json ./

# Install dependencies
RUN npm ci

# Copy source code
COPY . .

# Place ARG/ENV after COPY for optimal caching
ARG VITE_APP_NAME=FlowingFiles
ENV VITE_APP_NAME=${VITE_APP_NAME}

# Build the application
RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:alpine

# Copy custom nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built assets from build stage
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
